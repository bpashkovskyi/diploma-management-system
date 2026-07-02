namespace DiplomaManagementSystem.Application.Authorization.Contracts;

public interface IDiplomaAuthorizationService
{
    Task EnsureCanPerformAsync(
        Guid userId,
        Guid diplomaId,
        DiplomaAction action,
        CancellationToken cancellationToken = default);

    Task EnsureCanPerformAsync(
        Guid userId,
        Guid diplomaId,
        DiplomaAction action,
        Guid? expectedSessionId,
        CancellationToken cancellationToken = default);

    Task EnsureCanPerformOnTopicVersionAsync(
        Guid userId,
        Guid versionId,
        DiplomaAction action,
        CancellationToken cancellationToken = default);
}
