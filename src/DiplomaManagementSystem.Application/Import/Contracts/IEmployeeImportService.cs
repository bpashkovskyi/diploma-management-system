using DiplomaManagementSystem.Application.Import.Models;

namespace DiplomaManagementSystem.Application.Import.Contracts;

public interface IEmployeeImportService
{
    Task<ImportResult> ImportAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
}
