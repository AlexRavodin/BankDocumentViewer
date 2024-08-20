using Microsoft.EntityFrameworkCore;
using Viewer.Data.DbContext;
using Viewer.Models;
using Viewer.Models.Dto;

namespace Viewer.Services;

public class DataProvider : IDataProvider
{
    private readonly IAppDbContextFactory _dbContextFactory;

    public DataProvider(IAppDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task SaveGeneratedRecords(List<GeneratedRecord> records)
    {
        await using var context = _dbContextFactory.CreateDbContext();

        await context.GeneratedRecords.AddRangeAsync(records);

        await context.SaveChangesAsync();
    }

    public async Task SaveBankDocumentFile(ParsingResultDto parsingResultDto)
    {
        await using var context = _dbContextFactory.CreateDbContext();

        BankDocumentFile bankDocumentFile = new()
        {
            Name = parsingResultDto.FileName,
            Created = DateOnly.FromDateTime(DateTime.Now),
        };

        var operationClasses = parsingResultDto.Classes.Select(c => new OperationClass
        {
            Name = c.Name,
            Number = c.Number,
            Operations = c.Operations.Select(o => new Operation
            {
                AccountingCode = o.AccountingCode,
                ActiveSaldoIn = o.ActiveSaldoIn,
                PassiveSaldoIn = o.PassiveSaldoIn,
                Debit = o.Debit,
                Credit = o.Credit,
                ActiveSaldoOut = o.ActiveSaldoOut,
                PassiveSaldoOut = o.PassiveSaldoOut,
                BankDocument = bankDocumentFile
            }).ToList()
        }).ToList();

        bankDocumentFile.Classes = operationClasses;
        context.BankDocumentFiles.Add(bankDocumentFile);

        await context.SaveChangesAsync();
    }

    public async Task<StatisticsDto> CalculateSumAndMedian()
    {
        await using var context = _dbContextFactory.CreateDbContext();
        
        var results = context.Database
            .SqlQuery<StatisticsDto>($"SELECT * FROM CalculateSumAndMedian()")
            .ToList();

        return results.First();
    }

    public async Task<List<BankDocumentListItemDto>> LoadFiles()
    {
        await using var context = _dbContextFactory.CreateDbContext();

        var files = await context.BankDocumentFiles
            .Select(d => new BankDocumentListItemDto
            {
                Id = d.Id,
                Name = d.Name,
                Created = d.Created,
                RecordsCount = d.Operations.Count
            }).ToListAsync();

        return files;
    }

    public async Task<List<OperationListItemDto>> LoadOperations(int fileId)
    {
        await using var context = _dbContextFactory.CreateDbContext();

        var files = await context.Operations.Include(o => o.BankDocument)
            .Where(o => o.BankDocument.Id == fileId)
            .Include(o => o.Class)
            .Select(o => new OperationListItemDto
            {
                AccountingCode = o.AccountingCode,
                ActiveSaldoIn = o.ActiveSaldoIn,
                PassiveSaldoIn = o.PassiveSaldoIn,
                Debit = o.Debit,
                Credit = o.Credit,
                ActiveSaldoOut = o.ActiveSaldoOut,
                PassiveSaldoOut = o.PassiveSaldoOut,
                ClassName = o.Class.Name
            }).ToListAsync();

        return files;
    }

    public async Task<string> LoadFileNameById(int id)
    {
        await using var context = _dbContextFactory.CreateDbContext();

        return await context.BankDocumentFiles.Where(d => d.Id == id).Select(d => d.Name).FirstAsync();
    }

    public int SelectedFileId { get; set; }
}