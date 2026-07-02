using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class SupervisorSelectionService
{
    public void RequestSupervisor(
        Diploma diploma,
        DefenceSession defenceSession,
        Guid supervisorId)
    {
        ArgumentNullException.ThrowIfNull(diploma);
        ArgumentNullException.ThrowIfNull(defenceSession);

        EnsureSessionWritable(defenceSession);

        if (diploma.SupervisorAssignmentStatus == SupervisorAssignmentStatus.Confirmed)
        {
            throw new DomainException("Supervisor is already confirmed.");
        }

        if (SupervisorAssignmentRules.HasPendingRequest(
                diploma.SupervisorAssignmentStatus,
                diploma.SupervisorId))
        {
            throw new DomainException("Supervisor request is already pending confirmation.");
        }

        diploma.SupervisorId = supervisorId;
        diploma.SupervisorAssignmentStatus = SupervisorAssignmentStatus.Pending;
        diploma.UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static void EnsureSessionWritable(DefenceSession defenceSession)
    {
        if (defenceSession.Status == DefenceSessionStatus.Archived)
        {
            throw new DomainException("Defence session is archived.");
        }
    }
}
