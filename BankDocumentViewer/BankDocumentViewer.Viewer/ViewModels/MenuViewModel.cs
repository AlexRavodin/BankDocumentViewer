using System.Windows.Input;
using Viewer.Commands;
using Viewer.Services;

namespace Viewer.ViewModels;

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