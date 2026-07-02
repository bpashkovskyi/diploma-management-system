using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain.Tests;

public sealed class SupervisorAssignmentRulesTests
{
    [Fact]
    public void HasPendingRequest_WhenPendingWithoutSupervisor_ReturnsFalse()
    {
        Assert.False(SupervisorAssignmentRules.HasPendingRequest(
            SupervisorAssignmentStatus.Pending,
            supervisorId: null));
    }

    [Fact]
    public void HasPendingRequest_WhenPendingWithSupervisor_ReturnsTrue()
    {
        Assert.True(SupervisorAssignmentRules.HasPendingRequest(
            SupervisorAssignmentStatus.Pending,
            supervisorId: Guid.NewGuid()));
    }

    [Fact]
    public void CanRequestSupervisor_WhenFreshDiploma_ReturnsTrue()
    {
        Assert.True(SupervisorAssignmentRules.CanRequestSupervisor(
            SupervisorAssignmentStatus.Pending,
            supervisorId: null));
    }
}
