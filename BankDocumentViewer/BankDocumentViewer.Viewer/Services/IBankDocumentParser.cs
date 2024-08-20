using BankDocumentViewer.Viewer.Data.Dto;

namespace BankDocumentViewer.Viewer.Services;

public interface IBankDocumentParser
{
    public ParsingResultDto Parse(string fullFilename);
}