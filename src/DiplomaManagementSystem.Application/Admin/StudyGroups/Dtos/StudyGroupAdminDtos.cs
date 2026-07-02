namespace DiplomaManagementSystem.Application.Admin.StudyGroups.Dtos;

public sealed record StudyGroupListItemDto(
    Guid Id,
    string Name,
    Guid DefenceSessionId,
    string SessionLabel,
    int StudentCount);

public sealed record StudyGroupFormDto(
    Guid? Id,
    Guid DefenceSessionId,
    string Name,
    string? SessionLabel = null);
