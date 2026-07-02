using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Student.Dtos;

public sealed record StudentAdmissionStepDto(
    AdmissionStep Step,
    bool IsPassing,
    bool IsCurrent,
    bool IsLocked);

public sealed record StudentTopicVersionDto(
    Guid VersionId,
    int VersionNumber,
    string Title,
    TopicVersionStatus Status,
    string? RejectionReason,
    DateTimeOffset SubmittedAt,
    DateTimeOffset? ReviewedAt,
    string? ReviewedByName,
    DateTimeOffset? SupervisorReviewedAt,
    string? SupervisorReviewedByName);

public sealed record SelectSupervisorDto(Guid DiplomaId, Guid SupervisorId);

public sealed record SubmitTopicDto(Guid DiplomaId, string Title);
