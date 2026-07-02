using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Persistence.Contracts;

public interface IDiplomaDocumentQueries
{
    Task<List<DiplomaDocument>> ListForDiplomaReadAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default);

    Task<int> GetNextVersionNumberAsync(
        Guid diplomaId,
        DiplomaDocumentKind kind,
        CancellationToken cancellationToken = default);

    Task<bool> HasStudentWorkAsync(Guid diplomaId, CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, DiplomaDocument>> GetLatestStudentWorkByDiplomaIdsAsync(
        IReadOnlyCollection<Guid> diplomaIds,
        CancellationToken cancellationToken = default);
}
