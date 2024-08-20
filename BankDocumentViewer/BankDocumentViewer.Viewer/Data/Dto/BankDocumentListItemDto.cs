namespace BankDocumentViewer.Viewer.Data.Dto;

public class BankDocumentListItemDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateOnly Created { get; set; }
    
    public int RecordsCount { get; set; }
}