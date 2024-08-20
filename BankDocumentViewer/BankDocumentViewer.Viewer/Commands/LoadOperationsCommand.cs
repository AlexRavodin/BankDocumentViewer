using System.Windows;
using Viewer.Services;
using Viewer.ViewModels;

namespace Viewer.Commands;

public class LoadOperationsCommand : AsyncCommandBase
{
    private readonly OperationsListViewModel _viewModel;

    private readonly IDataProvider _dataProvider;
    
    public LoadOperationsCommand(OperationsListViewModel viewModel, IDataProvider dataProvider)
    {
        _viewModel = viewModel;
        _dataProvider = dataProvider;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        _viewModel.IsLoading = true;
        try
        {
            var id = _dataProvider.SelectedFileId;

            _viewModel.Filename = await _dataProvider.LoadFileNameById(id);

            var operations = await _dataProvider.LoadOperations(id);
            _viewModel.UpdateOperations(operations);
            
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