using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class DiplomaWorkflowInvariantValidator
{
    public void Validate(Diploma diploma, IReadOnlyList<DiplomaTopicVersion> topicVersions)
    {
        ArgumentNullException.ThrowIfNull(diploma);
        ArgumentNullException.ThrowIfNull(topicVersions);

        ValidateSupervisorRequiredForLifecycle(diploma);

        foreach (DiplomaTopicVersion version in topicVersions)
        {
            ValidateTopicVersion(diploma, version);
        }
    }

    public void ValidateSupervisorRequiredForLifecycle(Diploma diploma)
    {
        ArgumentNullException.ThrowIfNull(diploma);

        if (diploma.LifecycleStatus < DiplomaLifecycleStatus.TopicInReview)
        {
            return;
        }

        EnsureSupervisorConfirmed(diploma);
    }

    public void ValidateTopicVersion(Diploma diploma, DiplomaTopicVersion version)
    {
        ArgumentNullException.ThrowIfNull(diploma);
        ArgumentNullException.ThrowIfNull(version);

        if (version.Status is TopicVersionStatus.PendingHead or TopicVersionStatus.Approved)
        {
            EnsureSupervisorReviewTrail(version);
            EnsureSupervisorConfirmed(diploma);

            if (version.SupervisorReviewedById != diploma.SupervisorId)
            {
                throw new DomainException(DiplomaWorkflowInvariantMessages.SupervisorReviewMismatch);
            }
        }
    }

    private static void EnsureSupervisorConfirmed(Diploma diploma)
    {
        if (diploma.SupervisorId is null)
        {
            throw new DomainException(DiplomaWorkflowInvariantMessages.SupervisorRequired);
        }

        if (diploma.SupervisorAssignmentStatus != SupervisorAssignmentStatus.Confirmed)
        {
            throw new DomainException(DiplomaWorkflowInvariantMessages.SupervisorNotConfirmed);
        }
    }

    private static void EnsureSupervisorReviewTrail(DiplomaTopicVersion version)
    {
        if (version.SupervisorReviewedById is null || version.SupervisorReviewedAt is null)
        {
            throw new DomainException(DiplomaWorkflowInvariantMessages.SupervisorReviewRequired);
        }
    }
}

internal static class DiplomaWorkflowInvariantMessages
{
    internal const string SupervisorRequired = "A confirmed supervisor is required for the current diploma workflow state.";

    internal const string SupervisorNotConfirmed = "Supervisor assignment must be confirmed before topic review can proceed.";

    internal const string SupervisorReviewRequired = "Approved or head-pending topics must include supervisor review metadata.";

    internal const string SupervisorReviewMismatch = "Supervisor review metadata does not match the assigned supervisor.";
}
