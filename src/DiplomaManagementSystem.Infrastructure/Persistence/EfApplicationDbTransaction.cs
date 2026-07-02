using DiplomaManagementSystem.Application.Persistence.Contracts;

using Microsoft.EntityFrameworkCore.Storage;

namespace DiplomaManagementSystem.Infrastructure.Persistence;

internal sealed class EfApplicationDbTransaction(IDbContextTransaction transaction) : IApplicationDbTransaction
{
    public Task CommitAsync(CancellationToken cancellationToken = default) =>
        transaction.CommitAsync(cancellationToken);

    public ValueTask DisposeAsync() => transaction.DisposeAsync();
}
