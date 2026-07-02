using DiplomaManagementSystem.Application.Secretary.Dtos;

namespace DiplomaManagementSystem.Application.Secretary.Contracts;

public interface ISecretaryDashboardService
{
    Task<SecretaryDashboardDto?> GetDashboardAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
