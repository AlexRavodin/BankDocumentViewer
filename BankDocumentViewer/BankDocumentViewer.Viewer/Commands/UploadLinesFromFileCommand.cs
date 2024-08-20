using System.Windows;
using Viewer.Models.Options;
using Viewer.Services;
using Viewer.ViewModels;

namespace Viewer.Commands;

public class UploadLinesFromFileCommand : AsyncCommandBase
{
    private readonly GenerateViewModel _viewModel;

    private readonly FilesOptions _fileOptions;
    private readonly GeneratingOptions _generatingOptions;

    private readonly IFileService _fileService;

    public UploadLinesFromFileCommand(GenerateViewModel viewModel, IFileService fileService, FilesOptions fileOptions, GeneratingOptions generatingOptions)
    {
        _viewModel = viewModel;
        _fileService = fileService;
        _fileOptions = fileOptions;
        _generatingOptions = generatingOptions;
    }

    private void ChangeProgress(int newProgress, int newLinesLoaded, int newLinesLeft)
    {
        _viewModel.Progress = newProgress;
        _viewModel.LinesLoaded = newLinesLoaded;
        _viewModel.LinesLeft = newLinesLeft;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            await _fileService.ReadAndSaveFile(_fileOptions, _generatingOptions, ChangeProgress);

            MessageBox.Show("Данные загружены.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка загрузки данных: {e.Message}.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            ChangeProgress(0, 0, 0);
        }
    }
}