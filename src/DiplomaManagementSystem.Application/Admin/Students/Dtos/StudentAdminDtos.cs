using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Admin.Students.Dtos;

public sealed record StudentListItemDto(
    Guid Id,
    string FullName,
    string Email,
    Guid DefenceSessionId,
    string SessionLabel,
    string StudyGroupName,
    DateTimeOffset CreatedAt);

public sealed record StudentFormDto(
    Guid? Id,
    string FullName,
    string Email,
    Guid DefenceSessionId,
    Guid StudyGroupId,
    DefenceSessionType? SessionType = null,
    string? SessionLabel = null);

public sealed record StudentDetailsDto(
    Guid Id,
    string FullName,
    string Email,
    Guid DefenceSessionId,
    DefenceSessionType SessionType,
    string SessionLabel,
    Guid StudyGroupId,
    string StudyGroupName,
    bool HasDiploma,
    DateTimeOffset CreatedAt);

public sealed record StudentSessionOptionDto(
    Guid Id,
    string Label,
    DefenceSessionType Type);

public sealed record StudentGroupOptionDto(
    Guid Id,
    string Name);
