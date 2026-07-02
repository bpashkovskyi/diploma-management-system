using DiplomaManagementSystem.Application.Secretary.Dtos;

namespace DiplomaManagementSystem.Application.Secretary.Contracts;

public interface ISecretaryDiplomaDetailsService
{
    Task<DiplomaDetailsDto?> GetDetailsAsync(
        Guid sessionId,
        Guid diplomaId,
        CancellationToken cancellationToken = default);
}
