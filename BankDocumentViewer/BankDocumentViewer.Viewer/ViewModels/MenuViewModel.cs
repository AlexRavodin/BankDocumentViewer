using System.Windows.Input;
using BankDocumentViewer.Viewer.Commands;
using BankDocumentViewer.Viewer.Services;

namespace BankDocumentViewer.Viewer.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public ICommand GoToGeneratingCommand { get; }
    public ICommand GoToFilesCommand { get; }

    public MenuViewModel(NavigationService<GenerateViewModel> generatingViewNavigationService,
        NavigationService<FilesListViewModel> filesViewNavigationService)
    {
        GoToGeneratingCommand = new NavigateCommand<GenerateViewModel>(generatingViewNavigationService);

        GoToFilesCommand = new NavigateCommand<FilesListViewModel>(filesViewNavigationService);
    }
}