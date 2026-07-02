using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Persistence.Contracts;

public interface ITopicVersionQueries
{
    Task<DiplomaTopicVersion?> GetLatestAsync(Guid diplomaId, CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, string>> GetApprovedTitlesAsync(
        IReadOnlyCollection<Guid> diplomaIds,
        CancellationToken cancellationToken = default);

    Task<List<DiplomaTopicVersion>> ListPendingHeadReviewAsync(
        IReadOnlyCollection<Guid> sessionIds,
        CancellationToken cancellationToken = default);

    Task<List<DiplomaTopicVersion>> ListPendingSupervisorReviewAsync(
        Guid supervisorId,
        CancellationToken cancellationToken = default);

    Task<DiplomaTopicVersion?> FindWritableAsync(
        Guid versionId,
        CancellationToken cancellationToken = default);

    Task<List<DiplomaTopicVersion>> ListForDiplomaWritableAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default);
}
