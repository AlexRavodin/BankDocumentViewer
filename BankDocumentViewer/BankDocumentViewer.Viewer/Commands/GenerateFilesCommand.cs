using System.Windows;
using BankDocumentViewer.Viewer.ViewModels;

namespace BankDocumentViewer.Viewer.Commands;

public class GenerateFilesCommand : AsyncCommandBase
{
    private readonly GenerateViewModel _viewModel;

    private readonly GeneratingOptions _generatingOptions;
    private readonly FilesOptions _filesOptions;

    private readonly IFileService _fileService;

    public GenerateFilesCommand(GenerateViewModel viewModel, IFileService fileService, FilesOptions filesOptions, GeneratingOptions generatingOptions)
    {
        _viewModel = viewModel;
        _fileService = fileService;
        _generatingOptions = generatingOptions;
        _filesOptions = filesOptions;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        var startDate = _viewModel.StartDate;
        var endDate = _viewModel.EndDate;

        if (startDate > endDate)
        {
            MessageBox.Show($"Дата начала должна быть меньше даты конца.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _generatingOptions.StartDate = startDate;
        _generatingOptions.EndDate = endDate;
        
        _viewModel.AreFilesCreating = true;

        try
        {
            await _fileService.WriteStringsToFiles(_filesOptions, _generatingOptions);
            await Task.Delay(1000);
            MessageBox.Show("Файлы сгенерированы.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка генерации файлов: {e.Message}.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            _viewModel.AreFilesCreating = false;
        }
    }
}