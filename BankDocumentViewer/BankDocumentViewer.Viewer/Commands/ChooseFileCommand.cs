using System.Windows;
using BankDocumentViewer.Viewer.Services;
using BankDocumentViewer.Viewer.ViewModels;

namespace BankDocumentViewer.Viewer.Commands;

public class ChooseFileCommand : CommandBase
{
    private readonly FilesListViewModel _viewModel;

    private readonly IDataProvider _dataProvider;
    private readonly NavigationService<OperationsListViewModel> _operationsListNavigationService;

    public ChooseFileCommand(NavigationService<OperationsListViewModel> operationsListNavigationService,
        FilesListViewModel viewModel, IDataProvider dataProvider)
    {
        _viewModel = viewModel;
        _dataProvider = dataProvider;

        _operationsListNavigationService = operationsListNavigationService;
    }

    public override void Execute(object parameter)
    {
        if (parameter is int fileId)
        {
            _dataProvider.SelectedFileId = fileId;
        
            _operationsListNavigationService.Navigate();
        }
        else
        {
            MessageBox.Show($"Ошибка перехода к файлу.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}