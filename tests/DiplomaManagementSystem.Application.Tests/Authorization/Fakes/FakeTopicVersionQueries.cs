using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Tests.Authorization.Fakes;

internal sealed class FakeTopicVersionQueries(DiplomaTopicVersion? version = null) : ITopicVersionQueries
{
    public DiplomaTopicVersion? Version { get; set; } = version;

    public Task<DiplomaTopicVersion?> FindWritableAsync(Guid versionId, CancellationToken cancellationToken = default) =>
        Task.FromResult(Version is not null && Version.Id == versionId ? Version : null);

    public Task<DiplomaTopicVersion?> GetLatestAsync(Guid diplomaId, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task<Dictionary<Guid, string>> GetApprovedTitlesAsync(
        IReadOnlyCollection<Guid> diplomaIds,
        CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task<List<DiplomaTopicVersion>> ListPendingHeadReviewAsync(
        IReadOnlyCollection<Guid> sessionIds,
        CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task<List<DiplomaTopicVersion>> ListPendingSupervisorReviewAsync(
        Guid supervisorId,
        CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task<List<DiplomaTopicVersion>> ListForDiplomaWritableAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();
}
