using DiplomaManagementSystem.Application.Import.Models;

namespace DiplomaManagementSystem.Application.Import.Contracts;

public interface IImportFileParser
{
    bool CanParse(string fileName);

    Task<ImportParseResult<StudentImportRow>> ParseStudentsAsync(Stream stream, string fileName, CancellationToken cancellationToken);

    Task<ImportParseResult<EmployeeImportRow>> ParseEmployeesAsync(Stream stream, string fileName, CancellationToken cancellationToken);
}
