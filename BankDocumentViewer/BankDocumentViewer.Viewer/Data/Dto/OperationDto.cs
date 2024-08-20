namespace Viewer.Models.Dto;

public class OperationDto
{
    public int AccountingCode { get; set; }
    
    public decimal ActiveSaldoIn { get; set; } 
    
    public decimal PassiveSaldoIn { get; set; } 
    
    public decimal Debit { get; set; } 
    
    public decimal Credit { get; set; } 
    
    public decimal ActiveSaldoOut { get; set; } 
    
    public decimal PassiveSaldoOut { get; set; }
}