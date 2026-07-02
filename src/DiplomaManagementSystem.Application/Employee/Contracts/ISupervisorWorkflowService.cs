using DiplomaManagementSystem.Application.Employee.Dtos;

namespace DiplomaManagementSystem.Application.Employee.Contracts;

public interface ISupervisorWorkflowService
{
    Task<IReadOnlyList<PendingStudentDto>> GetPendingStudentsAsync(
        Guid supervisorId,
        CancellationToken cancellationToken = default);

    Task ConfirmStudentAsync(Guid supervisorId, Guid diplomaId, CancellationToken cancellationToken = default);

    Task RejectStudentAsync(
        Guid supervisorId,
        SupervisorActionDto request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TopicReviewItemDto>> GetTopicReviewsAsync(
        Guid supervisorId,
        CancellationToken cancellationToken = default);

    Task ApproveTopicAsync(
        Guid supervisorId,
        ApproveTopicDto request,
        CancellationToken cancellationToken = default);

    Task RejectTopicAsync(
        Guid supervisorId,
        ReviewTopicDto request,
        CancellationToken cancellationToken = default);
}
