using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class TopicReviewService(DiplomaWorkflowInvariantValidator validator)
{
    private readonly DiplomaWorkflowInvariantValidator _validator = validator;

    public TopicReviewService()
        : this(new DiplomaWorkflowInvariantValidator())
    {
    }

    public void SupervisorApprove(DiplomaTopicVersion version, Guid reviewerId)
    {
        EnsureVersionPending(version, TopicVersionStatus.PendingSupervisor);
        version.Status = TopicVersionStatus.PendingHead;
        version.SupervisorReviewedAt = DateTimeOffset.UtcNow;
        version.SupervisorReviewedById = reviewerId;
        version.ReviewedAt = null;
        version.ReviewedById = null;
        version.RejectionReason = null;
        ValidateIfDiplomaLoaded(version);
    }

    public void SupervisorReject(DiplomaTopicVersion version, Guid reviewerId, string rejectionReason)
    {
        EnsureVersionPending(version, TopicVersionStatus.PendingSupervisor);
        EnsureRejectionReason(rejectionReason);
        version.Status = TopicVersionStatus.Rejected;
        version.RejectionReason = rejectionReason.Trim();
        version.ReviewedAt = DateTimeOffset.UtcNow;
        version.ReviewedById = reviewerId;
    }

    public void DepartmentHeadApprove(DiplomaTopicVersion version, Guid reviewerId)
    {
        EnsureVersionPending(version, TopicVersionStatus.PendingHead);
        version.Status = TopicVersionStatus.Approved;
        version.ReviewedAt = DateTimeOffset.UtcNow;
        version.ReviewedById = reviewerId;
        version.RejectionReason = null;
        ValidateIfDiplomaLoaded(version);
    }

    public void DepartmentHeadReject(DiplomaTopicVersion version, Guid reviewerId, string rejectionReason)
    {
        EnsureVersionPending(version, TopicVersionStatus.PendingHead);
        EnsureRejectionReason(rejectionReason);
        version.Status = TopicVersionStatus.Rejected;
        version.RejectionReason = rejectionReason.Trim();
        version.ReviewedAt = DateTimeOffset.UtcNow;
        version.ReviewedById = reviewerId;
    }

    private void ValidateIfDiplomaLoaded(DiplomaTopicVersion version)
    {
        if (version.Diploma is not null)
        {
            _validator.ValidateTopicVersion(version.Diploma, version);
        }
    }

    private static void EnsureVersionPending(DiplomaTopicVersion version, TopicVersionStatus expectedStatus)
    {
        ArgumentNullException.ThrowIfNull(version);

        if (version.Status != expectedStatus)
        {
            throw new DomainException("Topic version is not in the expected review state.");
        }
    }

    private static void EnsureRejectionReason(string rejectionReason)
    {
        if (string.IsNullOrWhiteSpace(rejectionReason))
        {
            throw new DomainException("Rejection reason is required.");
        }
    }
}
