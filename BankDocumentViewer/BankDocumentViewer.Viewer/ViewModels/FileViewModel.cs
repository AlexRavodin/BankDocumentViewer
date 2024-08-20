using System.Windows.Input;
using BankDocumentViewer.Viewer.Data.Dto;

namespace BankDocumentViewer.Viewer.ViewModels;

public class FileViewModel : ViewModelBase
{
    private readonly BankDocumentListItemDto _listItem;

    public int Id => _listItem.Id;
    public string Name => _listItem.Name;
    public DateOnly Created => _listItem.Created;
    public int RecordsCount => _listItem.RecordsCount;
    
    public ICommand ChooseFileCommand { get; }

    public FileViewModel(BankDocumentListItemDto listItem, ICommand chooseFileCommand)
    {
        _listItem = listItem;
        ChooseFileCommand = chooseFileCommand;
    }
}
