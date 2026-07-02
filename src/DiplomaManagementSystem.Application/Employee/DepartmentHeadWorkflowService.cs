using DiplomaManagementSystem.Application.Authorization;
using DiplomaManagementSystem.Application.Authorization.Contracts;
using DiplomaManagementSystem.Application.Common.Contracts;
using DiplomaManagementSystem.Application.Employee.Contracts;
using DiplomaManagementSystem.Application.Employee.Dtos;
using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;
using DiplomaManagementSystem.Domain.Services;

namespace DiplomaManagementSystem.Application.Employee;

internal sealed class DepartmentHeadWorkflowService(
    IApplicationDbContext dbContext,
    IAnnualRoleQueries annualRoleQueries,
    ITopicVersionQueries topicVersionQueries,
    IAdmissionStepQueries admissionStepQueries,
    IUserDisplayQueries userDisplayQueries,
    IArchiveGuard archiveGuard,
    IDiplomaAuthorizationService diplomaAuthorizationService,
    TopicReviewService topicReviewService,
    DiplomaLifecycleService diplomaLifecycleService) : IDepartmentHeadWorkflowService
{
    public async Task<IReadOnlyList<TopicReviewItemDto>> GetPendingTopicsAsync(
        Guid departmentHeadId,
        CancellationToken cancellationToken = default)
    {
        List<Guid> sessionIds = await annualRoleQueries.GetSessionIdsAsync(
            departmentHeadId,
            AnnualRoleType.DepartmentHead,
            cancellationToken);

        if (sessionIds.Count == 0)
        {
            return [];
        }

        List<DiplomaTopicVersion> versions = await topicVersionQueries.ListPendingHeadReviewAsync(
            sessionIds,
            cancellationToken);

        return await EmployeeDiplomaListProjection.MapTopicReviewItemsAsync(
            userDisplayQueries,
            versions,
            cancellationToken);
    }

    public async Task ApproveTopicAsync(
        Guid departmentHeadId,
        ApproveTopicDto request,
        CancellationToken cancellationToken = default)
    {
        await diplomaAuthorizationService.EnsureCanPerformOnTopicVersionAsync(
            departmentHeadId,
            request.VersionId,
            DiplomaAction.ApproveTopicAsDepartmentHead,
            cancellationToken);

        DiplomaTopicVersion version = await RequireWritableHeadTopicVersionAsync(request.VersionId, cancellationToken);

        topicReviewService.DepartmentHeadApprove(version, departmentHeadId);
        AddApprovalCommentIfPresent(version.Diploma, departmentHeadId, request.Comment);
        await DiplomaLifecycleHelper.RecalculateAsync(
            admissionStepQueries,
            topicVersionQueries,
            diplomaLifecycleService,
            version.Diploma,
            cancellationToken);
        version.Diploma.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RejectTopicAsync(
        Guid departmentHeadId,
        ReviewTopicDto request,
        CancellationToken cancellationToken = default)
    {
        await diplomaAuthorizationService.EnsureCanPerformOnTopicVersionAsync(
            departmentHeadId,
            request.VersionId,
            DiplomaAction.RejectTopicAsDepartmentHead,
            cancellationToken);

        DiplomaTopicVersion version = await RequireWritableHeadTopicVersionAsync(request.VersionId, cancellationToken);

        topicReviewService.DepartmentHeadReject(version, departmentHeadId, request.RejectionReason!);
        await DiplomaLifecycleHelper.RecalculateAsync(
            admissionStepQueries,
            topicVersionQueries,
            diplomaLifecycleService,
            version.Diploma,
            cancellationToken);
        version.Diploma.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<DiplomaTopicVersion> RequireWritableHeadTopicVersionAsync(
        Guid versionId,
        CancellationToken cancellationToken)
    {
        DiplomaTopicVersion? version = await topicVersionQueries.FindWritableAsync(versionId, cancellationToken);

        if (version is null)
        {
            throw new DomainException(AuthorizationMessages.TopicVersionNotFound);
        }

        archiveGuard.EnsureWritable(version.Diploma.DefenceSession);

        if (version.Status != TopicVersionStatus.PendingHead)
        {
            throw new DomainException(AuthorizationMessages.TopicVersionWrongState);
        }

        return version;
    }

    private void AddApprovalCommentIfPresent(Diploma diploma, Guid authorId, string? comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            return;
        }

        dbContext.DiplomaComments.Add(new DiplomaComment
        {
            Id = Guid.NewGuid(),
            DiplomaId = diploma.Id,
            AuthorId = authorId,
            Body = comment.Trim(),
            CreatedAt = DateTimeOffset.UtcNow,
        });
    }
}
