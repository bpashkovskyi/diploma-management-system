using DiplomaManagementSystem.Application.Employee.Dtos;

namespace DiplomaManagementSystem.Application.Employee.Contracts;

public interface IDepartmentHeadWorkflowService
{
    Task<IReadOnlyList<TopicReviewItemDto>> GetPendingTopicsAsync(
        Guid departmentHeadId,
        CancellationToken cancellationToken = default);

    Task ApproveTopicAsync(
        Guid departmentHeadId,
        ApproveTopicDto request,
        CancellationToken cancellationToken = default);

    Task RejectTopicAsync(
        Guid departmentHeadId,
        ReviewTopicDto request,
        CancellationToken cancellationToken = default);
}
