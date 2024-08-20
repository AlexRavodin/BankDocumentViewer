using System.Collections.ObjectModel;
using System.Windows.Input;
using BankDocumentViewer.Viewer.Commands;
using BankDocumentViewer.Viewer.Data.Dto;
using BankDocumentViewer.Viewer.Services;

namespace BankDocumentViewer.Viewer.ViewModels;

public class OperationsListViewModel : ViewModelBase
{
    private readonly ObservableCollection<OperationViewModel> _operations;

    public IEnumerable<OperationViewModel> Operations => _operations;
    
    private string _fileName;
    public string Filename
    {
        get
        {
            return _fileName;
        }
        set
        {
            _fileName = value;
            OnPropertyChanged(nameof(Filename));
        }
    }
    
    private bool _isLoading;
    public bool IsLoading
    {
        get
        {
            return _isLoading;
        }
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }
    
    public ICommand GoBackCommand { get; }
    public ICommand LoadOperationsCommand { get; }

    public OperationsListViewModel(NavigationService<FilesListViewModel> filesListViewNavigationService, IDataProvider dataProvider)
    {
        GoBackCommand = new NavigateCommand<FilesListViewModel>(filesListViewNavigationService);
        LoadOperationsCommand = new LoadOperationsCommand(this, dataProvider);

        _operations = new ObservableCollection<OperationViewModel>();
    }
    
    public static OperationsListViewModel LoadViewModel(NavigationService<FilesListViewModel> filesListViewNavigationService, IDataProvider dataProvider)
    {
        OperationsListViewModel viewModel = new OperationsListViewModel(filesListViewNavigationService, dataProvider);
        
        viewModel.LoadOperationsCommand.Execute(null);

        return viewModel;
    }
    
    public void UpdateOperations(List<OperationListItemDto> operations)
    {
        _operations.Clear();

        foreach (OperationListItemDto operation in operations)
        {
            var operationViewModel = new OperationViewModel(operation);
            _operations.Add(operationViewModel);
        }
    }
}