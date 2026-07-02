using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain;

public static class SupervisorAssignmentRules
{
    public static bool HasPendingRequest(SupervisorAssignmentStatus status, Guid? supervisorId) =>
        status == SupervisorAssignmentStatus.Pending && supervisorId.HasValue;

    public static bool CanRequestSupervisor(SupervisorAssignmentStatus status, Guid? supervisorId) =>
        status != SupervisorAssignmentStatus.Confirmed
        && !HasPendingRequest(status, supervisorId);
}
