using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Infrastructure.Persistence.Queries;

internal sealed class DiplomaDocumentQueries(ApplicationDbContext dbContext) : IDiplomaDocumentQueries
{
    public Task<List<DiplomaDocument>> ListForDiplomaReadAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.DiplomaDocuments
            .AsNoTracking()
            .Where(document => document.DiplomaId == diplomaId)
            .OrderBy(document => document.Kind)
            .ThenBy(document => document.VersionNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetNextVersionNumberAsync(
        Guid diplomaId,
        DiplomaDocumentKind kind,
        CancellationToken cancellationToken = default)
    {
        int? maxVersion = await dbContext.DiplomaDocuments
            .Where(document => document.DiplomaId == diplomaId && document.Kind == kind)
            .Select(document => (int?)document.VersionNumber)
            .MaxAsync(cancellationToken);

        return (maxVersion ?? 0) + 1;
    }

    public Task<bool> HasStudentWorkAsync(Guid diplomaId, CancellationToken cancellationToken = default)
    {
        return dbContext.DiplomaDocuments.AnyAsync(
            document => document.DiplomaId == diplomaId && document.Kind == DiplomaDocumentKind.StudentWork,
            cancellationToken);
    }

    public async Task<Dictionary<Guid, DiplomaDocument>> GetLatestStudentWorkByDiplomaIdsAsync(
        IReadOnlyCollection<Guid> diplomaIds,
        CancellationToken cancellationToken = default)
    {
        if (diplomaIds.Count == 0)
        {
            return [];
        }

        List<DiplomaDocument> documents = await dbContext.DiplomaDocuments
            .AsNoTracking()
            .Where(document => diplomaIds.Contains(document.DiplomaId)
                               && document.Kind == DiplomaDocumentKind.StudentWork)
            .ToListAsync(cancellationToken);

        return documents
            .GroupBy(document => document.DiplomaId)
            .ToDictionary(
                group => group.Key,
                group => group.OrderByDescending(document => document.VersionNumber).First());
    }
}
