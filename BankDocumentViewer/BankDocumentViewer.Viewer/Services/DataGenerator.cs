using System.Globalization;
using Viewer.Models.Options;

namespace Viewer.Services;

public class DataGenerator : IDataGenerator
{
    private GeneratingOptions Options { get; set; }

    private readonly Random _random;

    private const string RusChars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";

    private const string EngChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    private const int RussianCharByteSize = 2;
    private const int SeparatorByteSize = 4;
    private const int SeparatorsCount = 4;
    private const int NewLineCharSize = 1;
    private const int FloatConstPartByteSize = 9;
    private const int DateOnlyByteSize = 9;


    public DataGenerator(GeneratingOptions options)
    {
        Options = options;

        _random = new Random();
    }

    public List<string> GenerateStrings(int linesCount, GeneratingOptions options)
    {
        Options = options;
        var floatingPointDigits = options.DigitsAfterDotCount;
        var strings = new List<string>(linesCount);

        for (int i = 0; i < linesCount; i++)
        {
            var str = string.Join("||", GenerateDateOnly(), GenerateEngString(), GenerateRusString(), GenerateInteger(),
                GenerateFloatingPoint().ToString($"F{floatingPointDigits}"));

            strings.Add(str);
        }

        return strings;
    }

    public int GetApproximateLineCount(long bytesCount, GeneratingOptions options)
    {
        return (int)(bytesCount / (NewLineCharSize + SeparatorByteSize * SeparatorsCount + DateOnlyByteSize +
                             Options.NumberOfEnglishChars + RussianCharByteSize * Options.NumberOfRussianChars +
                             CalculateAverageDigitLength(Options.MinInteger, Options.MaxInteger) +
                             CalculateAverageDigitLength(Options.MinFloat, Options.MaxFloat) + FloatConstPartByteSize));
    }

    private DateOnly GenerateDateOnly()
    {
        var daysBetween = Options.EndDate.Subtract(Options.StartDate).Days;
        var randomDays = _random.Next(0, daysBetween);

        return DateOnly.FromDateTime(Options.StartDate.AddDays(randomDays));
    }

    private int GenerateInteger()
    {
        return _random.Next(Options.MinInteger, Options.MaxInteger + 1);
    }

    private float GenerateFloatingPoint()
    {
        return Options.MinFloat + _random.NextSingle() * (Options.MaxFloat - Options.MinFloat);
    }

    private string GenerateEngString()
    {
        var stringChars = new char[Options.NumberOfEnglishChars];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = EngChars[_random.Next(EngChars.Length)];
        }

        return new string(stringChars);
    }

    private string GenerateRusString()
    {
        var stringChars = new char[Options.NumberOfRussianChars];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = RusChars[_random.Next(RusChars.Length)];
        }

        return new string(stringChars);
    }
    
    private static double CalculateAverageDigitLength(int a, int b)
    {
        var totalDigits = 0;
        var count = 0;

        var minDigits = a.ToString().Length;
        var maxDigits = b.ToString().Length;

        for (int digits = minDigits; digits <= maxDigits; digits++)
        {
            var min = (int)Math.Pow(10, digits - 1);
            var max = (int)Math.Pow(10, digits) - 1;

            var rangeMin = Math.Max(min, a);
            var rangeMax = Math.Min(max, b);

            if (rangeMin <= rangeMax)
            {
                var rangeCount = rangeMax - rangeMin + 1;
                totalDigits += rangeCount * digits;
                count += rangeCount;
            }
        }

        return (double)totalDigits / count;
    }
    
    private static double CalculateAverageDigitLength(double a, double b)
    {
        var totalDigits = 0;
        var count = 0;

        var intA = (int)a;
        var intB = (int)b;
        

        var minDigits = intA.ToString(CultureInfo.InvariantCulture).Length;
        var maxDigits = intB.ToString(CultureInfo.InvariantCulture).Length;

        for (int digits = minDigits; digits <= maxDigits; digits++)
        {
            var min = (int)Math.Pow(10, digits - 1);
            var max = (int)Math.Pow(10, digits) - 1;

            var rangeMin = Math.Max(min, intA);
            var rangeMax = Math.Min(max, intB);

            if (rangeMin <= rangeMax)
            {
                var rangeCount = rangeMax - rangeMin + 1;
                totalDigits += rangeCount * digits;
                count += rangeCount;
            }
        }

        return (double)totalDigits / count;
    }

}