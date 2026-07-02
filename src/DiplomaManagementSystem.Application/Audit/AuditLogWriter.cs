using DiplomaManagementSystem.Application.Audit.Contracts;
using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Audit;

internal sealed class AuditLogWriter(IApplicationDbContext dbContext) : IAuditLogWriter
{
    public Task WriteAsync(AuditLogEntry entry, CancellationToken cancellationToken = default)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityType = entry.EntityType,
            EntityId = entry.EntityId,
            Action = entry.Action,
            OldValue = entry.OldValue,
            NewValue = entry.NewValue,
            PerformedById = entry.PerformedById,
            PerformedAt = DateTimeOffset.UtcNow,
            DefenceSessionId = entry.DefenceSessionId,
        });

        return Task.CompletedTask;
    }
}
