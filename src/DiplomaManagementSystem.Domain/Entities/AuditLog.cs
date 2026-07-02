namespace DiplomaManagementSystem.Domain.Entities;

public sealed class AuditLog
{
    public Guid Id { get; set; }

    public string EntityType { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public Guid PerformedById { get; set; }

    public DateTimeOffset PerformedAt { get; set; }

    public Guid? DefenceSessionId { get; set; }

    public DefenceSession? DefenceSession { get; set; }
}
