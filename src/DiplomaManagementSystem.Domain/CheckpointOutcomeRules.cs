using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain;

public static class CheckpointOutcomeRules
{
    public static bool IsPassing(CheckpointOutcome? outcome) =>
        outcome is CheckpointOutcome.Approved or CheckpointOutcome.ApprovedWithRemarks;

    public static bool RequiresComment(CheckpointOutcome outcome) =>
        outcome is CheckpointOutcome.NotApproved or CheckpointOutcome.ApprovedWithRemarks;
}
