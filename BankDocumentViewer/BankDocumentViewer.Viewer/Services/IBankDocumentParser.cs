using Viewer.Models.Dto;

namespace Viewer.Services;

public interface IBankDocumentParser
{
    public ParsingResultDto Parse(string fullFilename);
}