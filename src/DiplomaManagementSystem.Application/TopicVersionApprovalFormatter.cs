using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application;

public sealed record TopicApprovalDisplay(
    string? SupervisorLine,
    string? HeadLine);

public static class TopicVersionApprovalFormatter
{
    public static TopicApprovalDisplay? BuildApprovedDisplay(
        DiplomaTopicVersion approvedVersion,
        string? supervisorApproverName,
        string? headApproverName)
    {
        if (approvedVersion.Status != TopicVersionStatus.Approved)
        {
            return null;
        }

        string? supervisorLine = FormatSupervisorApprovalLine(
            supervisorApproverName,
            approvedVersion.SupervisorReviewedAt);

        string? headLine = null;

        if (!string.IsNullOrWhiteSpace(headApproverName) && approvedVersion.ReviewedAt.HasValue)
        {
            headLine = $"Завідувач: {headApproverName} — {FormatLocalDateTime(approvedVersion.ReviewedAt.Value)}";
        }

        return new TopicApprovalDisplay(supervisorLine, headLine);
    }

    public static string FormatSubmittedAt(DateTimeOffset submittedAt) =>
        $"Подано: {FormatLocalDateTime(submittedAt)}";

    public static string? FormatSupervisorLine(
        TopicVersionStatus status,
        string? diplomaSupervisorName,
        string? supervisorReviewedByName,
        DateTimeOffset? supervisorReviewedAt,
        string? reviewedByName,
        DateTimeOffset? reviewedAt)
    {
        return status switch
        {
            TopicVersionStatus.Approved => FormatSupervisorApprovalLine(
                supervisorReviewedByName ?? diplomaSupervisorName,
                supervisorReviewedAt),
            TopicVersionStatus.PendingHead => FormatSupervisorApprovalLine(
                supervisorReviewedByName ?? reviewedByName,
                supervisorReviewedAt ?? reviewedAt),
            _ => null,
        };
    }

    public static string? FormatHeadLine(
        TopicVersionStatus status,
        string? reviewedByName,
        DateTimeOffset? reviewedAt)
    {
        return status switch
        {
            TopicVersionStatus.Approved when !string.IsNullOrWhiteSpace(reviewedByName) && reviewedAt.HasValue =>
                $"Завідувач: {reviewedByName} — {FormatLocalDateTime(reviewedAt.Value)}",
            TopicVersionStatus.PendingHead => "Очікує погодження завідувачем",
            _ => null,
        };
    }

    public static string? FormatRejectionLine(
        TopicVersionStatus status,
        string? reviewedByName,
        DateTimeOffset? reviewedAt)
    {
        if (status != TopicVersionStatus.Rejected
            || string.IsNullOrWhiteSpace(reviewedByName)
            || !reviewedAt.HasValue)
        {
            return null;
        }

        return $"Відхилено: {reviewedByName} — {FormatLocalDateTime(reviewedAt.Value)}";
    }

    public static string? BuildTopicApprovedStepDetail(TopicApprovalDisplay? approval)
    {
        if (approval is null)
        {
            return "Виконано";
        }

        List<string> lines = ["Виконано"];

        if (!string.IsNullOrWhiteSpace(approval.SupervisorLine))
        {
            lines.Add(approval.SupervisorLine);
        }

        if (!string.IsNullOrWhiteSpace(approval.HeadLine))
        {
            lines.Add(approval.HeadLine);
        }

        return string.Join('\n', lines);
    }

    private static string? FormatSupervisorApprovalLine(string? approverName, DateTimeOffset? reviewedAt)
    {
        if (string.IsNullOrWhiteSpace(approverName))
        {
            return null;
        }

        return reviewedAt.HasValue
            ? $"Керівник: {approverName} — {FormatLocalDateTime(reviewedAt.Value)}"
            : $"Керівник: {approverName} — погоджено";
    }

    private static string FormatLocalDateTime(DateTimeOffset value) =>
        value.ToLocalTime().ToString("g");
}
