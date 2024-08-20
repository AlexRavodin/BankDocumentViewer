namespace Viewer.Models;

public class OperationClass
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int  Number { get; set; }
    
    public IEnumerable<Operation> Operations { get; set; }
}