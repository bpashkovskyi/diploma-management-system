using DiplomaManagementSystem.Application.Secretary.Dtos;

namespace DiplomaManagementSystem.Application.Secretary.Contracts;

public interface IAdmittedReportService
{
    Task<AdmittedReportDto?> GetReportAsync(Guid sessionId, CancellationToken cancellationToken = default);

    Task<byte[]> ExportCsvAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
