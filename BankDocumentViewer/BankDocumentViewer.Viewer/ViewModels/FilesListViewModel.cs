using System.Collections.ObjectModel;
using System.Windows.Input;
using Viewer.Commands;
using Viewer.Models.Dto;
using Viewer.Services;

namespace Viewer.ViewModels;

public class FilesListViewModel : ViewModelBase
{
    private readonly ObservableCollection<FileViewModel> _files;

    public IEnumerable<FileViewModel> Files => _files;

    private bool _isLoading;

    public bool IsLoading
    {
        get { return _isLoading; }
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    private bool _selectedFileId;

    public bool SelectedFileId
    {
        get { return _selectedFileId; }
        set
        {
            _selectedFileId = value;
            OnPropertyChanged(nameof(SelectedFileId));
        }
    }

    public ICommand LoadFilesCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand UploadOperationsFromFileCommand { get; }
    public ICommand ChooseFileCommand { get; }

    public FilesListViewModel(NavigationService<MenuViewModel> menuViewNavigationService,
        NavigationService<OperationsListViewModel> operationsListViewNavigationService, IDataProvider dataProvider, IBankDocumentParser parser)
    {
        GoBackCommand = new NavigateCommand<MenuViewModel>(menuViewNavigationService);
        LoadFilesCommand = new LoadFilesCommand(this, dataProvider);
        UploadOperationsFromFileCommand = new UploadOperationsFromFileCommand(this, parser, dataProvider);
        ChooseFileCommand = new ChooseFileCommand(operationsListViewNavigationService, this, dataProvider);

        _files = new ObservableCollection<FileViewModel>();
    }

    public static FilesListViewModel LoadViewModel(NavigationService<MenuViewModel> menuViewNavigationService,
        NavigationService<OperationsListViewModel> operationsListViewNavigationService, IDataProvider dataProvider,  IBankDocumentParser parser)
    {
        FilesListViewModel viewModel = new FilesListViewModel(menuViewNavigationService, operationsListViewNavigationService, dataProvider, parser);

        viewModel.LoadFilesCommand.Execute(null);

        return viewModel;
    }
    
    public void UpdateFiles(List<BankDocumentListItemDto> files)
    {
        _files.Clear();

        foreach (BankDocumentListItemDto file in files)
        {
            var reservationViewModel = new FileViewModel(file, ChooseFileCommand);
            _files.Add(reservationViewModel);
        }
    }
}