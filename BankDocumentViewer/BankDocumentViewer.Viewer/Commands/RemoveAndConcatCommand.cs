using System.Windows;
using Viewer.Models;
using Viewer.Models.Options;
using Viewer.Services;
using Viewer.ViewModels;

namespace Viewer.Commands;

public class RemoveAndConcatCommand : AsyncCommandBase
{
    private readonly GenerateViewModel _viewModel;

    private readonly IFileService _fileService;

    private readonly FilesOptions _options;

    public RemoveAndConcatCommand(GenerateViewModel viewModel, IFileService fileService, FilesOptions options)
    {
        _viewModel = viewModel;
        _fileService = fileService;
        _options = options;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        _options.SearchSubstring = _viewModel.SearchSubstring;
        _options.ResultFileName = _viewModel.ResultFileName;

        _viewModel.IsResultFileCreating = true;
        try
        {
            var deletedLinesCount = await _fileService.FilterAndConcat(_options);
            MessageBox.Show($"Файлы объединены. Было удалено {deletedLinesCount} строк.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка объединения файлов: {e.Message}.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            _viewModel.IsResultFileCreating = false;
        }
        
        
    }
}