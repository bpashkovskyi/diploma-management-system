namespace DiplomaManagementSystem.Application.Admin.Employees.Dtos;

public sealed record EmployeeListItemDto(
    Guid Id,
    string FullName,
    string Email,
    DateTimeOffset CreatedAt);

public sealed record EmployeeFormDto(
    Guid? Id,
    string FullName,
    string Email);

public sealed record EmployeeDetailsDto(
    Guid Id,
    string FullName,
    string Email,
    bool HasAssignments,
    DateTimeOffset CreatedAt);
