using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class SecretarySupervisorOverrideService(DiplomaWorkflowInvariantValidator validator)
{
    private readonly DiplomaWorkflowInvariantValidator _validator = validator;

    public SecretarySupervisorOverrideService()
        : this(new DiplomaWorkflowInvariantValidator())
    {
    }

    public void Override(
        Diploma diploma,
        DefenceSession defenceSession,
        Guid supervisorId)
    {
        SupervisorOverridePolicy.EnsureCanOverride(diploma, defenceSession);

        diploma.SupervisorId = supervisorId;
        diploma.SupervisorAssignmentStatus = SupervisorAssignmentStatus.Confirmed;
        diploma.UpdatedAt = DateTimeOffset.UtcNow;
        _validator.ValidateSupervisorRequiredForLifecycle(diploma);
    }
}
