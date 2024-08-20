using System.Windows;
using Viewer.Services;
using Viewer.ViewModels;

namespace Viewer.Commands;

public class LoadFilesCommand : AsyncCommandBase
{
    private readonly FilesListViewModel _viewModel;

    private readonly IDataProvider _dataProvider;
    
    public LoadFilesCommand(FilesListViewModel viewModel, IDataProvider dataProvider)
    {
        _viewModel = viewModel;
        _dataProvider = dataProvider;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        _viewModel.IsLoading = true;
        try
        {
            var files = await _dataProvider.LoadFiles();

            _viewModel.UpdateFiles(files);
            
            await Task.Delay(1000);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка объединения файлов: {e.Message}.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            _viewModel.IsLoading = false;
        }
    }
}