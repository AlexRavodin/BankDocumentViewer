namespace BankDocumentViewer.Viewer.ViewModels;

public class OperationViewModel
{
    private readonly OperationListItemDto _operation;

    public int AccountingCode => _operation.AccountingCode;
    public decimal ActiveSaldoIn => _operation.ActiveSaldoIn;
    public decimal PassiveSaldoIn => _operation.PassiveSaldoIn;
    public decimal Debit => _operation.Debit;
    public decimal Credit => _operation.Credit;
    public decimal ActiveSaldoOut => _operation.ActiveSaldoOut;
    public decimal PassiveSaldoOut => _operation.PassiveSaldoOut;
    public string ClassName => _operation.ClassName;

    public OperationViewModel(OperationListItemDto operation)
    {
        _operation = operation;
    }
}