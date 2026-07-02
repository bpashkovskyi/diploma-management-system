using DiplomaManagementSystem.Domain.Enums;

using DiplomaManagementSystem.Application.ReadModels;

namespace DiplomaManagementSystem.Application.Admin.AnnualRoles.Dtos;

public sealed record AnnualRoleSlotDto(
    AnnualRoleType RoleType,
    Guid? AssignedEmployeeId,
    string? AssignedEmployeeName);

public sealed record AnnualRolesPageDto(
    Guid DefenceSessionId,
    string SessionLabel,
    IReadOnlyList<AnnualRoleSlotDto> Roles,
    IReadOnlyList<PersonOptionDto> Employees);

public sealed record AssignAnnualRoleDto(
    Guid DefenceSessionId,
    AnnualRoleType RoleType,
    Guid EmployeeId);
