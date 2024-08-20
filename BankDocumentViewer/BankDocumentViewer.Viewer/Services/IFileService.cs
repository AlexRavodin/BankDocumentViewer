using BankDocumentViewer.Viewer.Data.Options;

namespace BankDocumentViewer.Viewer.Services;

public interface IFileService
{
    public Task WriteStringsToFiles(FilesOptions options, GeneratingOptions generatingOptions);

    public Task<int> FilterAndConcat(FilesOptions options);

    public Task ReadAndSaveFile(FilesOptions options, GeneratingOptions generatingOptions,
        Action<int, int, int> changeProgressDelegate);
}