using BankDocumentViewer.Viewer.Data.Options;

namespace BankDocumentViewer.Viewer.Services;

public interface IDataGenerator
{
    public List<string> GenerateStrings(int linesCount, GeneratingOptions options);

    public int GetApproximateLineCount(long bytesCount, GeneratingOptions options);
}