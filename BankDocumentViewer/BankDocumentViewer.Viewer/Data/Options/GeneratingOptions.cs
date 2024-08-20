namespace Viewer.Models.Options;

public class GeneratingOptions
{
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int NumberOfEnglishChars { get; set; }
    
    public int NumberOfRussianChars { get; set; }
    
    public int MinInteger { get; set; }
    
    public int MaxInteger { get; set; }
    
    public float MinFloat { get; set; }
    
    public float MaxFloat { get; set; }
    
    public int DigitsAfterDotCount { get; set; }
}