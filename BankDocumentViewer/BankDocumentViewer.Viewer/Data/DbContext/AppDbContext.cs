using Microsoft.EntityFrameworkCore;
using Viewer.Models;
using Viewer.Models.Dto;

namespace Viewer.Data.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<BankDocumentFile> BankDocumentFiles { get; set; }

    public DbSet<GeneratedRecord> GeneratedRecords { get; set; }

    public DbSet<Operation> Operations { get; set; }

    public IQueryable<StatisticsDto> CalculateSumAndMedian() => FromExpression(() => CalculateSumAndMedian());


    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatisticsDto>().HasNoKey();
        
        modelBuilder.HasDbFunction(() => CalculateSumAndMedian());
    }
}