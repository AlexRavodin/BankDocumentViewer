﻿using BankDocumentViewer.Viewer.Data.Dto;
using BankDocumentViewer.Viewer.Data.Entities;

namespace BankDocumentViewer.Viewer.Services;

public interface IDataProvider
{
    public Task SaveGeneratedRecords(List<GeneratedRecord> records);

    public Task SaveBankDocumentFile(ParsingResultDto parsingResultDto);
    
    public Task<StatisticsDto> CalculateSumAndMedian();
    
    public Task<List<BankDocumentListItemDto>> LoadFiles();
    
    public Task<List<OperationListItemDto>> LoadOperations(int fileId);

    public Task<string> LoadFileNameById(int id);

    public int SelectedFileId { get; set; }
}