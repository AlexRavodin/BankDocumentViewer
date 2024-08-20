using System.Windows.Input;
using Viewer.Commands;
using Viewer.Models;
using Viewer.Models.Options;
using Viewer.Services;

namespace Viewer.ViewModels;

public class GenerateViewModel : ViewModelBase
{
    public ICommand GoBackCommand { get; }
    public ICommand ShowStatisticsCommand { get; }
    public ICommand GenerateFilesCommand { get; }
    public ICommand RemoveAndConcatCommand { get; }
    public ICommand UploadLinesFromFileCommand { get; }

    private int _progress;

    public int Progress
    {
        get { return _progress; }
        set
        {
            _progress = value;
            OnPropertyChanged(nameof(Progress));
        }
    }

    private int _linesLoaded;

    public int LinesLoaded
    {
        get { return _linesLoaded; }
        set
        {
            _linesLoaded = value;
            OnPropertyChanged(nameof(LinesLoaded));
        }
    }

    private int _linesLeft;

    public int LinesLeft
    {
        get { return _linesLeft; }
        set
        {
            _linesLeft = value;
            OnPropertyChanged(nameof(LinesLeft));
        }
    }

    private bool _areFilesCreating;

    public bool AreFilesCreating
    {
        get { return _areFilesCreating; }
        set
        {
            _areFilesCreating = value;
            OnPropertyChanged(nameof(AreFilesCreating));
        }
    }

    private bool _isResultFileCreating;

    public bool IsResultFileCreating
    {
        get { return _isResultFileCreating; }
        set
        {
            _isResultFileCreating = value;
            OnPropertyChanged(nameof(IsResultFileCreating));
        }
    }

    private bool _isStatisticsLoading;

    public bool IsStatisticsLoading
    {
        get { return _isStatisticsLoading; }
        set
        {
            _isStatisticsLoading = value;
            OnPropertyChanged(nameof(IsStatisticsLoading));
        }
    }

    private string _pathToDirectory;

    public string PathToDirectory
    {
        get { return _pathToDirectory; }
        set
        {
            _pathToDirectory = value;
            OnPropertyChanged(nameof(PathToDirectory));
        }
    }

    private string _resultFileName;

    public string ResultFileName
    {
        get { return _resultFileName; }
        set
        {
            _resultFileName = value;
            OnPropertyChanged(nameof(ResultFileName));
        }
    }

    private DateTime _startDate;

    public DateTime StartDate
    {
        get { return _startDate; }
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
        }
    }

    private DateTime _endDate;

    public DateTime EndDate
    {
        get { return _endDate; }
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }

    private string _searchSubstring;

    public string SearchSubstring
    {
        get { return _searchSubstring; }
        set
        {
            _searchSubstring = value;
            OnPropertyChanged(nameof(SearchSubstring));
        }
    }

    public GenerateViewModel(NavigationService<MenuViewModel> menuViewNavigationService, IFileService fileService,
        GeneratingOptions generatingOptions, FilesOptions filesOptions, IDataProvider dataProvider)
    {
        StartDate = generatingOptions.StartDate;
        EndDate = generatingOptions.EndDate;

        PathToDirectory = filesOptions.TempDirPath;
        ResultFileName = filesOptions.ResultFileName;
        SearchSubstring = filesOptions.SearchSubstring;

        LinesLoaded = 0;
        LinesLeft = 0;
        Progress = 0;

        GoBackCommand = new NavigateCommand<MenuViewModel>(menuViewNavigationService);
        GenerateFilesCommand = new GenerateFilesCommand(this, fileService, filesOptions, generatingOptions);
        RemoveAndConcatCommand = new RemoveAndConcatCommand(this, fileService, filesOptions);
        UploadLinesFromFileCommand = new UploadLinesFromFileCommand(this, fileService, filesOptions, generatingOptions);
        ShowStatisticsCommand = new ShowStatisticsCommand(this, dataProvider);
    }
}