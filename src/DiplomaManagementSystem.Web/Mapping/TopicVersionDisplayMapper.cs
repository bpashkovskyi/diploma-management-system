using DiplomaManagementSystem.Application;
using DiplomaManagementSystem.Application.Secretary.Dtos;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Web.Areas.Secretary.Models;

namespace DiplomaManagementSystem.Web.Mapping;

internal static class TopicVersionDisplayMapper
{
    public static TopicVersionDetailViewModel MapSecretaryTopicVersion(
        SecretaryTopicVersionDto version,
        string? diplomaSupervisorName) =>
        Map(
            version.VersionNumber,
            version.Title,
            version.Status,
            version.RejectionReason,
            version.SubmittedAt,
            version.ReviewedByName,
            version.ReviewedAt,
            version.SupervisorReviewedAt,
            version.SupervisorReviewedByName,
            diplomaSupervisorName);

    public static TopicVersionDetailViewModel MapStudentTopicVersion(
        Application.Student.Dtos.StudentTopicVersionDto version,
        string? diplomaSupervisorName) =>
        Map(
            version.VersionNumber,
            version.Title,
            version.Status,
            version.RejectionReason,
            version.SubmittedAt,
            version.ReviewedByName,
            version.ReviewedAt,
            version.SupervisorReviewedAt,
            version.SupervisorReviewedByName,
            diplomaSupervisorName);

    private static TopicVersionDetailViewModel Map(
        int versionNumber,
        string title,
        TopicVersionStatus status,
        string? rejectionReason,
        DateTimeOffset submittedAt,
        string? reviewedByName,
        DateTimeOffset? reviewedAt,
        DateTimeOffset? supervisorReviewedAt,
        string? supervisorReviewedByName,
        string? diplomaSupervisorName) =>
        new()
        {
            VersionNumber = versionNumber,
            Title = title,
            StatusDisplay = UkrainianDisplay.FormatTopicVersionStatus(status),
            RejectionReason = rejectionReason,
            SubmittedAtDisplay = TopicVersionApprovalFormatter.FormatSubmittedAt(submittedAt),
            SupervisorApprovalLine = TopicVersionApprovalFormatter.FormatSupervisorLine(
                status,
                diplomaSupervisorName,
                supervisorReviewedByName,
                supervisorReviewedAt,
                reviewedByName,
                reviewedAt),
            HeadApprovalLine = TopicVersionApprovalFormatter.FormatHeadLine(
                status,
                reviewedByName,
                reviewedAt),
            RejectionLine = TopicVersionApprovalFormatter.FormatRejectionLine(
                status,
                reviewedByName,
                reviewedAt),
        };
}
