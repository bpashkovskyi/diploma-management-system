using DiplomaManagementSystem.Application.ReadModels;
using DiplomaManagementSystem.Application.Documents.Dtos;
using DiplomaManagementSystem.Application.Student;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Secretary.Dtos;

public sealed record DiplomaDetailsHeaderDto(
    Guid Id,
    Guid SessionId,
    DefenceSessionType SessionType,
    string StudentFullName,
    string StudentEmail,
    string StudyGroupName);

public sealed record DiplomaAssignmentsDto(
    Guid? SupervisorId,
    string? SupervisorName,
    SupervisorAssignmentStatus SupervisorAssignmentStatus,
    Guid? ReviewerId,
    string? ReviewerName,
    ReviewAssignmentStatus ReviewAssignmentStatus);

public sealed record DiplomaDetailsHistoryDto(
    IReadOnlyList<AdmissionStepStatusDto> AdmissionSteps,
    IReadOnlyList<AdmissionStepAttemptDto> AttemptHistory,
    IReadOnlyList<SecretaryTopicVersionDto> TopicVersions,
    IReadOnlyList<DiplomaCommentDto> Comments);

public sealed record DiplomaDetailsDto(
    DiplomaDetailsHeaderDto Header,
    DiplomaAssignmentsDto Assignments,
    DiplomaLifecycleSnapshotDto State,
    DiplomaDetailsHistoryDto History,
    DiplomaWorkflowSecretaryFlags Actions,
    StudentWorkflowProgressDto WorkflowProgress,
    DiplomaDocumentsBundleDto Documents,
    IReadOnlyList<PersonOptionDto> EmployeePool);
