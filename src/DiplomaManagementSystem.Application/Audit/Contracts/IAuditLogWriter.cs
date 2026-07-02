namespace DiplomaManagementSystem.Application.Audit.Contracts;

public interface IAuditLogWriter
{
    Task WriteAsync(AuditLogEntry entry, CancellationToken cancellationToken = default);
}

public sealed record AuditLogEntry(
    Guid PerformedById,
    string EntityType,
    Guid EntityId,
    string Action,
    string? OldValue,
    string? NewValue,
    Guid? DefenceSessionId);
