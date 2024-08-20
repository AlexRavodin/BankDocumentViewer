namespace BankDocumentViewer.Viewer.Data.DbContext;

public interface IAppDbContextFactory
{
    AppDbContext CreateDbContext();
}