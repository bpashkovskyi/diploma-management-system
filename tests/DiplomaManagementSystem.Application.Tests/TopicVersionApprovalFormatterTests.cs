using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Tests;

public sealed class TopicVersionApprovalFormatterTests
{
    private static readonly DateTimeOffset ReviewedAt = new(2026, 3, 15, 10, 30, 0, TimeSpan.Zero);

    // TC-APP-HLP-011
    [Fact]
    public void BuildApprovedDisplay_ReturnsLines()
    {
        DiplomaTopicVersion version = CreateApprovedVersion();

        TopicApprovalDisplay? display = TopicVersionApprovalFormatter.BuildApprovedDisplay(
            version,
            supervisorApproverName: "Петро Петренко",
            headApproverName: "Олена Коваленко");

        Assert.NotNull(display);
        Assert.Contains("Петро Петренко", display.SupervisorLine, StringComparison.Ordinal);
        Assert.Contains("Олена Коваленко", display.HeadLine, StringComparison.Ordinal);
    }

    // TC-APP-HLP-012
    [Fact]
    public void BuildApprovedDisplay_NotApproved_ReturnsNull()
    {
        DiplomaTopicVersion version = CreateApprovedVersion();
        version.Status = TopicVersionStatus.PendingSupervisor;

        TopicApprovalDisplay? display = TopicVersionApprovalFormatter.BuildApprovedDisplay(
            version,
            supervisorApproverName: "Петро Петренко",
            headApproverName: null);

        Assert.Null(display);
    }

    // TC-APP-HLP-013
    [Fact]
    public void FormatRejectionLine_ReturnsLine()
    {
        string? line = TopicVersionApprovalFormatter.FormatRejectionLine(
            TopicVersionStatus.Rejected,
            reviewedByName: "Петро Петренко",
            reviewedAt: ReviewedAt);

        Assert.Contains("Відхилено", line, StringComparison.Ordinal);
        Assert.Contains("Петро Петренко", line, StringComparison.Ordinal);
    }

    // TC-APP-HLP-014
    [Fact]
    public void FormatHeadLine_PendingHead_ReturnsMessage()
    {
        string? line = TopicVersionApprovalFormatter.FormatHeadLine(
            TopicVersionStatus.PendingHead,
            reviewedByName: null,
            reviewedAt: null);

        Assert.Equal("Очікує погодження завідувачем", line);
    }

    // TC-APP-HLP-015
    [Fact]
    public void BuildTopicApprovedStepDetail_IncludesLines()
    {
        TopicApprovalDisplay approval = new("Керівник: Петро — погоджено", "Завідувач: Олена — 15.03.2026");

        string? detail = TopicVersionApprovalFormatter.BuildTopicApprovedStepDetail(approval);

        Assert.NotNull(detail);
        Assert.Contains("Виконано", detail, StringComparison.Ordinal);
        Assert.Contains("Петро", detail, StringComparison.Ordinal);
        Assert.Contains("Олена", detail, StringComparison.Ordinal);
    }

    [Fact]
    public void BuildApprovedDisplay_WithoutHeadApprover_OmitsHeadLine()
    {
        DiplomaTopicVersion version = CreateApprovedVersion();

        TopicApprovalDisplay? display = TopicVersionApprovalFormatter.BuildApprovedDisplay(
            version,
            supervisorApproverName: "Петро Петренко",
            headApproverName: null);

        Assert.NotNull(display);
        Assert.NotNull(display.SupervisorLine);
        Assert.Null(display.HeadLine);
    }

    [Fact]
    public void FormatHeadLine_Approved_ReturnsFormattedLine()
    {
        string? line = TopicVersionApprovalFormatter.FormatHeadLine(
            TopicVersionStatus.Approved,
            reviewedByName: "Олена Коваленко",
            reviewedAt: ReviewedAt);

        Assert.Contains("Олена Коваленко", line, StringComparison.Ordinal);
        Assert.Contains("Завідувач", line, StringComparison.Ordinal);
    }

    [Fact]
    public void FormatSupervisorLine_Approved_UsesReviewerName()
    {
        string? line = TopicVersionApprovalFormatter.FormatSupervisorLine(
            TopicVersionStatus.Approved,
            diplomaSupervisorName: "Керівник з диплома",
            supervisorReviewedByName: "Петро Петренко",
            supervisorReviewedAt: ReviewedAt,
            reviewedByName: null,
            reviewedAt: null);

        Assert.Contains("Петро Петренко", line, StringComparison.Ordinal);
    }

    [Fact]
    public void FormatSupervisorLine_PendingHead_FallsBackToReviewedBy()
    {
        string? line = TopicVersionApprovalFormatter.FormatSupervisorLine(
            TopicVersionStatus.PendingHead,
            diplomaSupervisorName: null,
            supervisorReviewedByName: null,
            supervisorReviewedAt: null,
            reviewedByName: "Петро Петренко",
            reviewedAt: ReviewedAt);

        Assert.Contains("Петро Петренко", line, StringComparison.Ordinal);
    }

    [Fact]
    public void FormatRejectionLine_WhenNotRejected_ReturnsNull()
    {
        Assert.Null(TopicVersionApprovalFormatter.FormatRejectionLine(
            TopicVersionStatus.PendingSupervisor,
            reviewedByName: "Петро Петренко",
            reviewedAt: ReviewedAt));
    }

    [Fact]
    public void FormatSubmittedAt_IncludesLocalTimestamp()
    {
        string formatted = TopicVersionApprovalFormatter.FormatSubmittedAt(ReviewedAt);

        Assert.StartsWith("Подано:", formatted, StringComparison.Ordinal);
    }

    [Fact]
    public void BuildTopicApprovedStepDetail_WhenApprovalNull_ReturnsCompletedOnly()
    {
        Assert.Equal("Виконано", TopicVersionApprovalFormatter.BuildTopicApprovedStepDetail(null));
    }

    private static DiplomaTopicVersion CreateApprovedVersion() => new()
    {
        Id = Guid.NewGuid(),
        VersionNumber = 1,
        Title = "Тема",
        Status = TopicVersionStatus.Approved,
        SubmittedAt = ReviewedAt,
        SupervisorReviewedAt = ReviewedAt,
        ReviewedAt = ReviewedAt,
    };
}
