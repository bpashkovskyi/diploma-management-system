namespace DiplomaManagementSystem.Application.Persistence;

public sealed record DiplomaWritableCriteria(
    Guid DiplomaId,
    Guid? StudentId = null,
    Guid? SupervisorId = null,
    Guid? SessionId = null,
    bool IncludeAdmissionAttempts = false,
    bool IncludeTopicVersions = false);
