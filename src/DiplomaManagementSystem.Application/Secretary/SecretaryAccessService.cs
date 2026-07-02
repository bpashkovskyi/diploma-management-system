using DiplomaManagementSystem.Application.Persistence;
using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Application.Secretary.Contracts;
using DiplomaManagementSystem.Application.Secretary.Dtos;

namespace DiplomaManagementSystem.Application.Secretary;

public sealed class SecretaryAccessService(IAnnualRoleQueries annualRoleQueries) : ISecretaryAccessService
{
    public Task<bool> IsSecretaryAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return annualRoleQueries.IsSecretaryAsync(userId, cancellationToken);
    }

    public Task<bool> CanAccessSessionAsync(
        Guid userId,
        Guid defenceSessionId,
        CancellationToken cancellationToken = default)
    {
        return annualRoleQueries.CanAccessSessionAsSecretaryAsync(userId, defenceSessionId, cancellationToken);
    }

    public async Task<IReadOnlyList<SecretarySessionOptionDto>> GetAccessibleSessionsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        List<SecretarySessionRow> rows = await annualRoleQueries.ListAccessibleSecretarySessionsAsync(
            userId,
            cancellationToken);

        return rows
            .Select(row => new SecretarySessionOptionDto(
                row.Id,
                SecretarySessionLabel.Format(row.Year, row.Type, row.Semester)))
            .ToList();
    }
}
