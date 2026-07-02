using DiplomaManagementSystem.Application.Secretary.Dtos;

namespace DiplomaManagementSystem.Application.Secretary.Contracts;

public interface ISecretaryAccessService
{
    Task<bool> IsSecretaryAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> CanAccessSessionAsync(Guid userId, Guid defenceSessionId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SecretarySessionOptionDto>> GetAccessibleSessionsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
