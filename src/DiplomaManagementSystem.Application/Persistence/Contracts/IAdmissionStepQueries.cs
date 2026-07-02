using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Persistence.Contracts;

public interface IAdmissionStepQueries
{
    Task<List<DiplomaAdmissionStepAttempt>> ListForDiplomaAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default);

    Task<DiplomaAdmissionStepAttempt?> FindWritableAsync(
        Guid diplomaId,
        Guid attemptId,
        CancellationToken cancellationToken = default);
}
