using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain.Entities;

public sealed class AnnualRoleAssignment
{
    public Guid Id { get; set; }

    public Guid DefenceSessionId { get; set; }

    public DefenceSession DefenceSession { get; set; } = null!;

    public Guid EmployeeId { get; set; }

    public AnnualRoleType RoleType { get; set; }

    public DateTimeOffset AssignedAt { get; set; }
}
