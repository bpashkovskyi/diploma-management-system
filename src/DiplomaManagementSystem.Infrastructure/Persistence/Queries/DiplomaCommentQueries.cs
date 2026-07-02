using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Infrastructure.Persistence.Queries;

internal sealed class DiplomaCommentQueries(ApplicationDbContext dbContext) : IDiplomaCommentQueries
{
    public Task<List<DiplomaComment>> ListForDiplomaReadAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.DiplomaComments
            .AsNoTracking()
            .Where(comment => comment.DiplomaId == diplomaId)
            .OrderByDescending(comment => comment.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
