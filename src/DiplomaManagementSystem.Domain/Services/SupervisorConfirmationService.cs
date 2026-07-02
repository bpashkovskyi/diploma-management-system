using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class SupervisorConfirmationService(DiplomaWorkflowInvariantValidator validator)
{
    private readonly DiplomaWorkflowInvariantValidator _validator = validator;

    public SupervisorConfirmationService()
        : this(new DiplomaWorkflowInvariantValidator())
    {
    }

    public void Confirm(Diploma diploma, DefenceSession defenceSession, Guid supervisorId)
    {
        EnsureCanAct(diploma, defenceSession, supervisorId);
        diploma.SupervisorAssignmentStatus = SupervisorAssignmentStatus.Confirmed;
        diploma.UpdatedAt = DateTimeOffset.UtcNow;
        _validator.ValidateSupervisorRequiredForLifecycle(diploma);
    }

    public void Reject(Diploma diploma, DefenceSession defenceSession, Guid supervisorId)
    {
        EnsureCanAct(diploma, defenceSession, supervisorId);
        diploma.SupervisorAssignmentStatus = SupervisorAssignmentStatus.Rejected;
        diploma.UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static void EnsureCanAct(Diploma diploma, DefenceSession defenceSession, Guid supervisorId)
    {
        ArgumentNullException.ThrowIfNull(diploma);
        ArgumentNullException.ThrowIfNull(defenceSession);

        if (defenceSession.Status == DefenceSessionStatus.Archived)
        {
            throw new DomainException("Defence session is archived.");
        }

        if (diploma.SupervisorId != supervisorId)
        {
            throw new DomainException("You are not the supervisor for this diploma.");
        }

        if (diploma.SupervisorAssignmentStatus != SupervisorAssignmentStatus.Pending)
        {
            throw new DomainException("Supervisor assignment is not pending confirmation.");
        }
    }
}
