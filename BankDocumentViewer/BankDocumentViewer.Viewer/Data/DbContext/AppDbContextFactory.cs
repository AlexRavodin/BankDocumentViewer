using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Viewer.Data.DbContext;

public class AppDbContextFactory : IAppDbContextFactory
{
    private readonly string _connectionString;

    public AppDbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public AppDbContext CreateDbContext()
    {
        DbContextOptions options = new DbContextOptionsBuilder().UseNpgsql(_connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information).Options;

        return new AppDbContext(options);
    }
}