namespace BankDocumentViewer.Viewer.Data.Entities;

public class Operation
{
    public int Id { get; set; }
    
    public int AccountingCode { get; set; }
    
    public decimal ActiveSaldoIn { get; set; } 
    
    public decimal PassiveSaldoIn { get; set; } 
    
    public decimal Debit { get; set; } 
    
    public decimal Credit { get; set; } 
    
    public decimal ActiveSaldoOut { get; set; } 
    
    public decimal PassiveSaldoOut { get; set; }
    
    public OperationClass Class { get; set; }
    
    public BankDocumentFile BankDocument { get; set; }
}