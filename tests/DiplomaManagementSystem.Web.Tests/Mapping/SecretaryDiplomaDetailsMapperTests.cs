using DiplomaManagementSystem.Web.Areas.Secretary.Models;
using DiplomaManagementSystem.Web.Mapping;

namespace DiplomaManagementSystem.Web.Tests.Mapping;

public sealed class SecretaryDiplomaDetailsMapperTests
{
    // TC-WEB-MAP-001
    [Fact]
    public void Map_MapsHeaderAssignmentsAndState()
    {
        DiplomaDetailsViewModel viewModel = SecretaryDiplomaDetailsMapper.Map(MapperTestFixtures.CreateDiplomaDetailsDto());

        Assert.Equal(MapperTestFixtures.DiplomaId, viewModel.Id);
        Assert.Equal(MapperTestFixtures.SessionId, viewModel.SessionId);
        Assert.Equal("Іван Іваненко", viewModel.StudentFullName);
        Assert.Equal("Петро Петренко", viewModel.SupervisorName);
        Assert.Equal(MapperTestFixtures.SupervisorId, viewModel.SupervisorId);
        Assert.Equal(MapperTestFixtures.ReviewerId, viewModel.ReviewerId);
        Assert.NotNull(viewModel.LifecycleDisplay);
        Assert.NotNull(viewModel.CurrentAdmissionStepDisplay);
    }

    // TC-WEB-MAP-001b
    [Fact]
    public void Map_MapsHistoryActionsWorkflowAndDocuments()
    {
        DiplomaDetailsViewModel viewModel = SecretaryDiplomaDetailsMapper.Map(MapperTestFixtures.CreateDiplomaDetailsDto());

        Assert.Single(viewModel.AdmissionSteps);
        Assert.Single(viewModel.AttemptHistory);
        Assert.Single(viewModel.TopicVersions);
        Assert.Single(viewModel.Comments);
        Assert.Single(viewModel.TopicHistory);
        Assert.True(viewModel.CanAssignReviewer);
        Assert.False(viewModel.CanAdmit);
        Assert.Equal("not ready", viewModel.AdmitBlockedReason);
        Assert.NotNull(viewModel.WorkflowProgress);
        Assert.Equal("Поточний етап:", viewModel.WorkflowProgress!.CurrentStepHintLabel);
        Assert.Single(viewModel.Documents.StudentWorkVersions);
        Assert.Single(viewModel.EmployeePool);
    }

    // TC-WEB-MAP-001c
    [Fact]
    public void Map_FormatsTopicVersionApprovalLines()
    {
        DiplomaDetailsViewModel viewModel = SecretaryDiplomaDetailsMapper.Map(MapperTestFixtures.CreateDiplomaDetailsDto());

        TopicVersionDetailViewModel version = Assert.Single(viewModel.TopicVersions);
        Assert.Contains("Петро", version.SupervisorApprovalLine, StringComparison.Ordinal);
        Assert.Contains("Завідувач", version.HeadApprovalLine, StringComparison.Ordinal);
    }
}
