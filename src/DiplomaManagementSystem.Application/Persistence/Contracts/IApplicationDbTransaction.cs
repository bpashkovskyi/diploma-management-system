namespace DiplomaManagementSystem.Application.Persistence.Contracts;

public interface IApplicationDbTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
