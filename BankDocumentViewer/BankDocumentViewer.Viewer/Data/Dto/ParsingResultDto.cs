namespace BankDocumentViewer.Viewer.Data.Dto;

public class ParsingResultDto
{
    public string FileName { get; set; }
    
    public List<OperationClassDto> Classes { get; set; }
}