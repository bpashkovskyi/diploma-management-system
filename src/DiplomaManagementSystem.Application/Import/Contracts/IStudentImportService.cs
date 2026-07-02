using DiplomaManagementSystem.Application.Import.Models;

namespace DiplomaManagementSystem.Application.Import.Contracts;

public interface IStudentImportService
{
    Task<ImportResult> ImportAsync(
        Guid defenceSessionId,
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default);
}
