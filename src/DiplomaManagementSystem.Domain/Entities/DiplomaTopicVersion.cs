using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain.Entities;

public sealed class DiplomaTopicVersion
{
    public Guid Id { get; set; }

    public Guid DiplomaId { get; set; }

    public Diploma Diploma { get; set; } = null!;

    public int VersionNumber { get; set; }

    public string Title { get; set; } = string.Empty;

    public TopicVersionStatus Status { get; set; }

    public string? RejectionReason { get; set; }

    public DateTimeOffset SubmittedAt { get; set; }

    public DateTimeOffset? ReviewedAt { get; set; }

    public Guid? ReviewedById { get; set; }

    public DateTimeOffset? SupervisorReviewedAt { get; set; }

    public Guid? SupervisorReviewedById { get; set; }
}
