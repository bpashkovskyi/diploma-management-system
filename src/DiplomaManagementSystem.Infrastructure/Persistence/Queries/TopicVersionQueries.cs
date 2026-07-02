using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Infrastructure.Persistence.Queries;

internal sealed class TopicVersionQueries(ApplicationDbContext dbContext) : ITopicVersionQueries
{
    public Task<DiplomaTopicVersion?> GetLatestAsync(Guid diplomaId, CancellationToken cancellationToken = default)
    {
        return dbContext.DiplomaTopicVersions
            .Where(version => version.DiplomaId == diplomaId)
            .OrderByDescending(version => version.VersionNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Dictionary<Guid, string>> GetApprovedTitlesAsync(
        IReadOnlyCollection<Guid> diplomaIds,
        CancellationToken cancellationToken = default)
    {
        if (diplomaIds.Count == 0)
        {
            return [];
        }

        List<DiplomaTopicVersion> versions = await dbContext.DiplomaTopicVersions
            .AsNoTracking()
            .Where(version => diplomaIds.Contains(version.DiplomaId)
                              && version.Status == TopicVersionStatus.Approved)
            .ToListAsync(cancellationToken);

        return versions
            .GroupBy(version => version.DiplomaId)
            .ToDictionary(
                group => group.Key,
                group => group.OrderByDescending(version => version.VersionNumber).First().Title);
    }

    public Task<List<DiplomaTopicVersion>> ListPendingHeadReviewAsync(
        IReadOnlyCollection<Guid> sessionIds,
        CancellationToken cancellationToken = default)
    {
        if (sessionIds.Count == 0)
        {
            return Task.FromResult<List<DiplomaTopicVersion>>([]);
        }

        return dbContext.DiplomaTopicVersions
            .AsNoTracking()
            .Include(version => version.Diploma)
            .ThenInclude(diploma => diploma.DefenceSession)
            .Where(version => version.Status == TopicVersionStatus.PendingHead
                              && sessionIds.Contains(version.Diploma.DefenceSessionId)
                              && version.Diploma.DefenceSession.Status == DefenceSessionStatus.Active)
            .OrderBy(version => version.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<List<DiplomaTopicVersion>> ListPendingSupervisorReviewAsync(
        Guid supervisorId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.DiplomaTopicVersions
            .AsNoTracking()
            .Include(version => version.Diploma)
            .ThenInclude(diploma => diploma.DefenceSession)
            .Where(version => version.Diploma.SupervisorId == supervisorId
                              && version.Status == TopicVersionStatus.PendingSupervisor
                              && version.Diploma.DefenceSession.Status == DefenceSessionStatus.Active)
            .OrderBy(version => version.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<DiplomaTopicVersion?> FindWritableAsync(
        Guid versionId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.DiplomaTopicVersions
            .Include(version => version.Diploma)
            .ThenInclude(diploma => diploma.DefenceSession)
            .FirstOrDefaultAsync(version => version.Id == versionId, cancellationToken);
    }

    public Task<List<DiplomaTopicVersion>> ListForDiplomaWritableAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.DiplomaTopicVersions
            .Where(version => version.DiplomaId == diplomaId)
            .ToListAsync(cancellationToken);
    }
}
