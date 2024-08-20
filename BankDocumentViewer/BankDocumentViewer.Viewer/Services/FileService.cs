using System.IO;
using Viewer.Models;
using Viewer.Models.Options;

namespace Viewer.Services;

public class FileService : IFileService
{
    private readonly IDataGenerator _generator;

    private readonly IDataProvider _dataProvider;

    private readonly SemaphoreSlim _semaphore;

    private int _deletedLinesCount;

    private const int MaxLinesReadCount = 100;
    private const int MaxProgress = 100;
    private const int ZeroFilesLeft = 0;

    public FileService(IDataGenerator generator, IDataProvider dataProvider)
    {
        _generator = generator;
        _dataProvider = dataProvider;

        _semaphore = new SemaphoreSlim(1);
        _deletedLinesCount = 0;
    }

    public async Task WriteStringsToFiles(FilesOptions fileOptions, GeneratingOptions generatingOptions)
    {
        var tasks = new Task[fileOptions.FileCount];

        Directory.CreateDirectory(fileOptions.TempDirPath);

        for (int i = 0; i < fileOptions.FileCount; i++)
        {
            var filename = Path.Combine(fileOptions.TempDirPath, $"dataFile{i}.txt");

            var lines = _generator.GenerateStrings(fileOptions.LinesCount, generatingOptions);

            tasks[i] = File.WriteAllLinesAsync(filename, lines);
        }

        await Task.WhenAll(tasks);
    }

    public async Task<int> FilterAndConcat(FilesOptions options)
    {
        var tasks = new Task[options.FileCount];
        var resultFilePath = GetResultFilePath(options);

        await CreateResultFile(resultFilePath);

        for (int i = 0; i < options.FileCount; i++)
        {
            var filename = Path.Combine(options.TempDirPath, $"dataFile{i}.txt");

            Console.WriteLine(i);

            tasks[i] = WriteToResultFile(filename, resultFilePath, options);
        }

        await Task.WhenAll(tasks);

        return _deletedLinesCount;
    }

    public async Task ReadAndSaveFile(FilesOptions options, GeneratingOptions generatingOptions,
        Action<int, int, int> changeProgressDelegate)
    {
        var linesReadCount = 0;
        var currentLineIndex = 0;
        var records = new GeneratedRecord[MaxLinesReadCount];
        var resultFilePath = GetResultFilePath(options);
        var fileSize = GetFileSize(resultFilePath);

        var approximateLinesCount = _generator.GetApproximateLineCount(fileSize, generatingOptions);
        changeProgressDelegate(0, 0, approximateLinesCount);

        foreach (var line in File.ReadLines(resultFilePath))
        {
            if (!TryParseRecord(line, out records[currentLineIndex])) continue;

            if (currentLineIndex == MaxLinesReadCount - 1)
            {
                await _dataProvider.SaveGeneratedRecords(records.ToList());

                currentLineIndex = 0;
                var stringsLeft = approximateLinesCount - linesReadCount;

                var progress = GetPercentage(linesReadCount, approximateLinesCount);
                changeProgressDelegate(progress, linesReadCount,
                    stringsLeft > 0 ? stringsLeft : 0);
            }
            else
            {
                currentLineIndex++;
            }

            linesReadCount++;
        }

        changeProgressDelegate(MaxProgress, linesReadCount, ZeroFilesLeft);
    }

    private async Task WriteToResultFile(string fromFilename, string resultFilePath, FilesOptions options)
    {
        var lines = await File.ReadAllLinesAsync(fromFilename);
        var filteredLines = lines.Where(line => !line.Contains(options.SearchSubstring)).ToArray();

        await _semaphore.WaitAsync();

        _deletedLinesCount += lines.Length - filteredLines.Length;
        await File.AppendAllLinesAsync(resultFilePath, filteredLines);

        _semaphore.Release();
    }

    private static int GetPercentage(int linesReadCount, int approximateLinesCount)
    {
        var percentage = (double)linesReadCount / approximateLinesCount * 100;
        var progress = (int)Math.Clamp(percentage, 0, 100);

        return progress;
    }

    private static bool TryParseRecord(string input, out GeneratedRecord record)
    {
        record = null!;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var parts = input.Split(["||"], StringSplitOptions.None);

        if (parts.Length != 5)
        {
            return false;
        }

        if (!DateTime.TryParse(parts[0], out DateTime date))
        {
            return false;
        }

        date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

        var englishString = parts[1];
        var russianString = parts[2];

        if (!int.TryParse(parts[3], out var integerNumber))
        {
            return false;
        }

        if (!float.TryParse(parts[4], out var floatNumber))
        {
            return false;
        }

        record = new GeneratedRecord
        {
            Date = date,
            EnglishString = englishString,
            RussianString = russianString,
            IntegerNumber = integerNumber,
            FloatNumber = floatNumber
        };

        return true;
    }

    private static async Task CreateResultFile(string resultFilePath)
    {
        if (File.Exists(resultFilePath))
        {
            File.Delete(resultFilePath);
        }

        var file = File.Create(resultFilePath);

        await file.DisposeAsync();
    }

    private static string GetResultFilePath(FilesOptions options)
    {
        return Path.Combine(options.TempDirPath, options.ResultFileName);
    }

    private static long GetFileSize(string resultFilePath)
    {
        var fileInfo = new FileInfo(resultFilePath);
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException("File does not exist.");
        }

        return fileInfo.Length;
    }
}