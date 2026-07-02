using DiplomaManagementSystem.Application.Student;
using DiplomaManagementSystem.Web.Models.Shared;

namespace DiplomaManagementSystem.Web.Mapping;

internal static class WorkflowProgressMapper
{
    public static WorkflowProgressViewModel Map(
        StudentWorkflowProgressDto progress,
        string currentStepHintLabel = "Наступний крок:")
    {
        return new WorkflowProgressViewModel
        {
            ProgressPercent = progress.ProgressPercent,
            CompletedSteps = progress.CompletedCount,
            TotalSteps = progress.TotalCount,
            CurrentStepHintLabel = currentStepHintLabel,
            CurrentStepHint = progress.CurrentStepHint,
            Steps = progress.Steps
                .Select(step => new WorkflowStepViewModel
                {
                    Order = step.Order,
                    Title = step.Title,
                    State = step.State,
                    StateCssClass = step.State switch
                    {
                        StudentWorkflowStepState.Completed => "completed",
                        StudentWorkflowStepState.Current => "current",
                        _ => "upcoming",
                    },
                    Detail = step.Detail,
                    Metadata = BuildMetadata(step.Status),
                    Comment = step.Status?.Comment,
                    IsSecretaryOverride = step.Status?.IsSecretaryOverride ?? false,
                })
                .ToList(),
        };
    }

    private static string? BuildMetadata(StudentWorkflowStepStatusDto? status)
    {
        if (status is null)
        {
            return null;
        }

        if (status.CompletedByName is null
            && !status.CompletedAt.HasValue
            && !status.Outcome.HasValue)
        {
            return null;
        }

        List<string> parts = [];
        if (!string.IsNullOrWhiteSpace(status.CompletedByName))
        {
            parts.Add(status.CompletedByName);
        }

        if (status.CompletedAt.HasValue)
        {
            parts.Add(status.CompletedAt.Value.ToLocalTime().ToString("g"));
        }

        if (status.Outcome.HasValue)
        {
            parts.Add(UkrainianDisplay.FormatCheckpointOutcome(status.Outcome.Value));
        }

        return string.Join(" · ", parts);
    }
}
