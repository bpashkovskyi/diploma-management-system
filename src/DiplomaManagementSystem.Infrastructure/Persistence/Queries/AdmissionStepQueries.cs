using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Infrastructure.Persistence.Queries;

internal sealed class AdmissionStepQueries(ApplicationDbContext dbContext) : IAdmissionStepQueries
{
    public Task<List<DiplomaAdmissionStepAttempt>> ListForDiplomaAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.DiplomaAdmissionStepAttempts
            .AsNoTracking()
            .Where(attempt => attempt.DiplomaId == diplomaId)
            .ToListAsync(cancellationToken);
    }

    public Task<DiplomaAdmissionStepAttempt?> FindWritableAsync(
        Guid diplomaId,
        Guid attemptId,
        CancellationToken cancellationToken = default)
    {
        DiplomaAdmissionStepAttempt? tracked = dbContext.DiplomaAdmissionStepAttempts.Local
            .FirstOrDefault(attempt => attempt.Id == attemptId && attempt.DiplomaId == diplomaId);

        if (tracked is not null)
        {
            return Task.FromResult<DiplomaAdmissionStepAttempt?>(tracked);
        }

        return dbContext.DiplomaAdmissionStepAttempts
            .FirstOrDefaultAsync(
                attempt => attempt.Id == attemptId && attempt.DiplomaId == diplomaId,
                cancellationToken);
    }
}
