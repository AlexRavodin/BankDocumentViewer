namespace BankDocumentViewer.Viewer.Data.Entities;

public class BankDocumentFile
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateOnly Created { get; set; }
    
    public List<Operation> Operations { get; set; }
    
    public List<OperationClass> Classes { get; set; }
}