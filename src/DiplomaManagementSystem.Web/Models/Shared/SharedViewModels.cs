using DiplomaManagementSystem.Application.Student;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Web.Models.Shared;

public sealed class StudentAdmissionStepViewModel
{
    public AdmissionStep Step { get; set; }

    public string StepDisplay { get; set; } = string.Empty;

    public bool IsPassing { get; set; }

    public bool IsCurrent { get; set; }

    public bool IsLocked { get; set; }
}

public sealed class TopicVersionItemViewModel
{
    public Guid VersionId { get; set; }

    public int VersionNumber { get; set; }

    public string Title { get; set; } = string.Empty;

    public TopicVersionStatus Status { get; set; }

    public string StatusDisplay { get; set; } = string.Empty;

    public string? RejectionReason { get; set; }

    public DateTimeOffset SubmittedAt { get; set; }

    public DateTimeOffset? ReviewedAt { get; set; }
}

public sealed class CommentItemViewModel
{
    public string AuthorName { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class TopicHistoryEntryViewModel
{
    public int VersionNumber { get; set; }

    public string Title { get; set; } = string.Empty;

    public string StatusDisplay { get; set; } = string.Empty;

    public string? SubmittedAtDisplay { get; set; }

    public DateTimeOffset? SubmittedAt { get; set; }

    public string? RejectionReason { get; set; }

    public string? SupervisorApprovalLine { get; set; }

    public string? HeadApprovalLine { get; set; }

    public string? RejectionLine { get; set; }
}

public sealed class WorkflowProgressViewModel
{
    public int ProgressPercent { get; set; }

    public int CompletedSteps { get; set; }

    public int TotalSteps { get; set; }

    public string CurrentStepHintLabel { get; set; } = "Наступний крок:";

    public string? CurrentStepHint { get; set; }

    public IReadOnlyList<WorkflowStepViewModel> Steps { get; set; } = [];
}

public sealed class WorkflowStepViewModel
{
    public int Order { get; set; }

    public string Title { get; set; } = string.Empty;

    public StudentWorkflowStepState State { get; set; }

    public string StateCssClass { get; set; } = string.Empty;

    public string? Detail { get; set; }

    public string? StatusBadgeText { get; set; }

    public string? StatusBadgeClass { get; set; }

    public string? Metadata { get; set; }

    public string? Comment { get; set; }

    public bool IsSecretaryOverride { get; set; }
}
