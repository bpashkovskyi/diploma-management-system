using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Persistence.Contracts;

public interface IDefenceSessionQueries
{
    Task<bool> ExistsAsync(Guid sessionId, CancellationToken cancellationToken = default);

    Task<DefenceSessionType?> GetTypeAsync(Guid sessionId, CancellationToken cancellationToken = default);

    Task<DefenceSession?> FindReadAsync(Guid sessionId, CancellationToken cancellationToken = default);

    Task<DefenceSessionSummary?> FindSummaryAsync(Guid sessionId, CancellationToken cancellationToken = default);

    Task<DefenceSessionSummary?> FindForStudentAsync(Guid studentId, CancellationToken cancellationToken = default);
}
