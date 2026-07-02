using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain.Entities;

public sealed class DiplomaAdmissionStepAttempt
{
    public Guid Id { get; set; }

    public Guid DiplomaId { get; set; }

    public Diploma Diploma { get; set; } = null!;

    public AdmissionStep Step { get; set; }

    public int AttemptNumber { get; set; }

    public CheckpointOutcome Outcome { get; set; }

    public string? Comment { get; set; }

    public Guid RecordedById { get; set; }

    public DateTimeOffset RecordedAt { get; set; }

    public bool IsSecretaryOverride { get; set; }
}
