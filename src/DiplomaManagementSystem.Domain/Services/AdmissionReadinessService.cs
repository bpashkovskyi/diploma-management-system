using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class AdmissionReadinessService
{
    public bool IsReadyForAdmission(
        Diploma diploma,
        DiplomaTopicVersion? latestTopicVersion,
        IEnumerable<DiplomaAdmissionStepAttempt> attempts)
    {
        ArgumentNullException.ThrowIfNull(diploma);
        ArgumentNullException.ThrowIfNull(attempts);

        if (diploma.SupervisorAssignmentStatus != SupervisorAssignmentStatus.Confirmed)
        {
            return false;
        }

        if (latestTopicVersion is null || latestTopicVersion.Status != TopicVersionStatus.Approved)
        {
            return false;
        }

        if (!AdmissionStepStatusResolver.AreAllOutcomeStepsPassing(attempts))
        {
            return false;
        }

        if (diploma.ReviewAssignmentStatus != ReviewAssignmentStatus.Completed)
        {
            return false;
        }

        return diploma.CurrentAdmissionStep is null;
    }
}
