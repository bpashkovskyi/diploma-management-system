using System.Text;
using DiplomaManagementSystem.Application.Import.Contracts;
using DiplomaManagementSystem.Application.Import.Models;

namespace DiplomaManagementSystem.Application.Import;

internal sealed class ImportFileParser : IImportFileParser
{
    private const int StudentColumnCount = 3;
    private const int EmployeeColumnCount = 2;

    public bool CanParse(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension is ".csv" or ".xlsx";
    }

    public Task<ImportParseResult<StudentImportRow>> ParseStudentsAsync(
        Stream stream,
        string fileName,
        CancellationToken cancellationToken)
    {
        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        ImportParseResult<StudentImportRow> result = extension == ".xlsx"
            ? ParseStudentsFromXlsx(stream)
            : ParseStudentsFromCsv(stream);

        return Task.FromResult(result);
    }

    public Task<ImportParseResult<EmployeeImportRow>> ParseEmployeesAsync(
        Stream stream,
        string fileName,
        CancellationToken cancellationToken)
    {
        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        ImportParseResult<EmployeeImportRow> result = extension == ".xlsx"
            ? ParseEmployeesFromXlsx(stream)
            : ParseEmployeesFromCsv(stream);

        return Task.FromResult(result);
    }

    private static ImportParseResult<StudentImportRow> ParseStudentsFromCsv(Stream stream)
    {
        using StreamReader reader = new(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        List<StudentImportRow> rows = [];
        List<string> parseErrors = [];
        bool isHeader = true;
        int lineNumber = 0;

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();
            lineNumber++;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (isHeader)
            {
                isHeader = false;
                if (line.Contains("email", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            string[] parts = CsvLineTokenizer.Split(line);
            if (parts.Length < StudentColumnCount)
            {
                parseErrors.Add(ImportMessages.InsufficientColumns(lineNumber, StudentColumnCount, parts.Length));
                continue;
            }

            rows.Add(new StudentImportRow(parts[0].Trim(), parts[1].Trim().ToLowerInvariant(), parts[2].Trim()));
        }

        return new ImportParseResult<StudentImportRow> { Rows = rows, ParseErrors = parseErrors };
    }

    private static ImportParseResult<EmployeeImportRow> ParseEmployeesFromCsv(Stream stream)
    {
        using StreamReader reader = new(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        List<EmployeeImportRow> rows = [];
        List<string> parseErrors = [];
        bool isHeader = true;
        int lineNumber = 0;

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();
            lineNumber++;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (isHeader)
            {
                isHeader = false;
                if (line.Contains("email", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            string[] parts = CsvLineTokenizer.Split(line);
            if (parts.Length < EmployeeColumnCount)
            {
                parseErrors.Add(ImportMessages.InsufficientColumns(lineNumber, EmployeeColumnCount, parts.Length));
                continue;
            }

            rows.Add(new EmployeeImportRow(parts[0].Trim(), parts[1].Trim().ToLowerInvariant()));
        }

        return new ImportParseResult<EmployeeImportRow> { Rows = rows, ParseErrors = parseErrors };
    }

    private static ImportParseResult<StudentImportRow> ParseStudentsFromXlsx(Stream stream)
    {
        using ClosedXML.Excel.XLWorkbook workbook = new(stream);
        ClosedXML.Excel.IXLWorksheet sheet = workbook.Worksheet(1);
        List<StudentImportRow> rows = [];
        List<string> parseErrors = [];
        bool isHeader = true;

        foreach (ClosedXML.Excel.IXLRow row in sheet.RowsUsed())
        {
            int rowNumber = row.RowNumber();

            if (isHeader)
            {
                isHeader = false;
                string firstCell = row.Cell(1).GetString();
                if (firstCell.Contains("ПІБ", StringComparison.OrdinalIgnoreCase)
                    || firstCell.Contains("name", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            string fullName = row.Cell(1).GetString().Trim();
            string email = row.Cell(2).GetString().Trim().ToLowerInvariant();
            string group = row.Cell(3).GetString().Trim();

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                parseErrors.Add(ImportMessages.MissingRequiredFields(rowNumber));
                continue;
            }

            rows.Add(new StudentImportRow(fullName, email, group));
        }

        return new ImportParseResult<StudentImportRow> { Rows = rows, ParseErrors = parseErrors };
    }

    private static ImportParseResult<EmployeeImportRow> ParseEmployeesFromXlsx(Stream stream)
    {
        using ClosedXML.Excel.XLWorkbook workbook = new(stream);
        ClosedXML.Excel.IXLWorksheet sheet = workbook.Worksheet(1);
        List<EmployeeImportRow> rows = [];
        List<string> parseErrors = [];
        bool isHeader = true;

        foreach (ClosedXML.Excel.IXLRow row in sheet.RowsUsed())
        {
            int rowNumber = row.RowNumber();

            if (isHeader)
            {
                isHeader = false;
                string firstCell = row.Cell(1).GetString();
                if (firstCell.Contains("ПІБ", StringComparison.OrdinalIgnoreCase)
                    || firstCell.Contains("name", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            string fullName = row.Cell(1).GetString().Trim();
            string email = row.Cell(2).GetString().Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                parseErrors.Add(ImportMessages.MissingRequiredFields(rowNumber));
                continue;
            }

            rows.Add(new EmployeeImportRow(fullName, email));
        }

        return new ImportParseResult<EmployeeImportRow> { Rows = rows, ParseErrors = parseErrors };
    }
}
