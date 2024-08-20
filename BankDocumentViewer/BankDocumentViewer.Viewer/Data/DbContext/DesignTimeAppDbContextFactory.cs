using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Viewer.Data.DbContext
{
    public class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseNpgsql("Host=localhost;Port=5432;Database=db;Username=postgres;Password=1234")
                .LogTo(Console.WriteLine, LogLevel.Information)
                .Options;

            return new AppDbContext(options);
        }
    }
}