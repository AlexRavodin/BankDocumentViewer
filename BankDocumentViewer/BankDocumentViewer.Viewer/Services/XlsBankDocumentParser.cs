using System.IO;
using BankDocumentViewer.Viewer.Data.Dto;
using NPOI.HSSF.UserModel;

namespace BankDocumentViewer.Viewer.Services;

public class XlsBankDocumentParser : IBankDocumentParser
{
    public ParsingResultDto Parse(string fullFilename)
    {
        var classes = new List<OperationClassDto>();

        using (var fileReader = new FileStream(fullFilename, FileMode.Open, FileAccess.Read))
        {
            var workbook = new HSSFWorkbook(fileReader);
            var sheet = workbook.GetSheetAt(0);
            OperationClassDto? currentClass = null;

            for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);

                var cellValue = row?.GetCell(0)?.ToString()?.Trim();
                if (cellValue is null) continue;
                
                if (cellValue.StartsWith("класс", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (currentClass != null)
                    {
                        classes.Add(currentClass);
                    }
                    currentClass = ExtractOperation(cellValue);
                    continue;
                }

                if (currentClass == null || !int.TryParse(cellValue, out var code) || cellValue.Length <= 3) continue;

                var operation = new OperationDto
                {
                    AccountingCode = code,
                    ActiveSaldoIn = Convert.ToDecimal(row.GetCell(1)
                        ?.NumericCellValue),
                    PassiveSaldoIn = Convert.ToDecimal(row.GetCell(2)
                        ?.NumericCellValue),
                    Debit = Convert.ToDecimal(row.GetCell(3)
                        ?.NumericCellValue),
                    Credit = Convert.ToDecimal(row.GetCell(4)
                        ?.NumericCellValue),
                    ActiveSaldoOut = Convert.ToDecimal(row.GetCell(4)
                        ?.NumericCellValue),
                    PassiveSaldoOut = Convert.ToDecimal(row.GetCell(4)
                        ?.NumericCellValue),
                };
                
                currentClass.Operations.Add(operation);
            }

            if (currentClass != null)
            {
                classes.Add(currentClass);
            }
        }

        return new ParsingResultDto
        {
            FileName = Path.GetFileName(fullFilename),
            Classes = classes,
        };
    }
    
    private OperationClassDto ExtractOperation(string input)
    {
        var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length <= 2 || parts[0] != "КЛАСС")
        {
            throw new ArgumentException("Wrong class name row.");
        }

        if (!int.TryParse(parts[1], out var number))
        {
            throw new ArgumentException("Wrong class name row.");
        }

        var name = string.Join(" ", parts.Skip(2));
        
        return new OperationClassDto
        {
            Number = number,
            Name = name
        };
    } 
}