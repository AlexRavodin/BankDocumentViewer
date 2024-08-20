using Viewer.Models.Options;

namespace Viewer.Services;

public interface IDataGenerator
{
    public List<string> GenerateStrings(int linesCount, GeneratingOptions options);

    public int GetApproximateLineCount(long bytesCount, GeneratingOptions options);
}