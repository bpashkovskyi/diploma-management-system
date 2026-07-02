using DiplomaManagementSystem.Application.Documents.Dtos;
using DiplomaManagementSystem.Application.ReadModels;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Student.Dtos;

public sealed record MyDiplomaHeaderDto(
    Guid? DiplomaId,
    bool HasDiploma,
    string? SessionLabel,
    DefenceSessionType? SessionType);

public sealed record MyDiplomaAssignmentsDto(
    string? SupervisorName,
    Guid? SupervisorId,
    SupervisorAssignmentStatus? SupervisorAssignmentStatus,
    string? CurrentTopicTitle,
    TopicVersionStatus? TopicStatus);

public sealed record MyDiplomaHistoryDto(
    IReadOnlyList<StudentAdmissionStepDto> Checkpoints,
    IReadOnlyList<StudentTopicVersionDto> TopicVersions,
    IReadOnlyList<DiplomaCommentDto> Comments);

public sealed record MyDiplomaDto(
    MyDiplomaHeaderDto Header,
    MyDiplomaAssignmentsDto Assignments,
    DiplomaLifecycleSnapshotDto? State,
    MyDiplomaHistoryDto History,
    DiplomaWorkflowStudentFlags? Actions,
    StudentWorkflowProgressDto? WorkflowProgress,
    DiplomaDocumentsBundleDto? Documents,
    IReadOnlyList<PersonOptionDto> SupervisorPool);
