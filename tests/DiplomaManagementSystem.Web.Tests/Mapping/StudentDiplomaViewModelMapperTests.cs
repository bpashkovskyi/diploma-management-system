using DiplomaManagementSystem.Web.Areas.Student.Models;
using DiplomaManagementSystem.Web.Mapping;

namespace DiplomaManagementSystem.Web.Tests.Mapping;

public sealed class StudentDiplomaViewModelMapperTests
{
    // TC-WEB-MAP-002
    [Fact]
    public void Map_MapsAllCompositeSections()
    {
        MyDiplomaViewModel viewModel = StudentDiplomaViewModelMapper.Map(MapperTestFixtures.CreateMyDiplomaDto());

        Assert.Equal(MapperTestFixtures.DiplomaId, viewModel.DiplomaId);
        Assert.True(viewModel.HasDiploma);
        Assert.Equal("Петро Петренко", viewModel.SupervisorName);
        Assert.Equal("Тема роботи", viewModel.CurrentTopicTitle);
        Assert.Single(viewModel.AdmissionSteps);
        Assert.Single(viewModel.TopicVersions);
        Assert.Single(viewModel.Comments);
        Assert.True(viewModel.CanDeclareWorkReady);
        Assert.True(viewModel.CanUploadWork);
        Assert.NotNull(viewModel.WorkflowProgress);
        Assert.Equal(8, viewModel.WorkflowProgress!.TotalSteps);
        Assert.NotNull(viewModel.Documents);
        Assert.Single(viewModel.Documents!.StudentWorkVersions);
        Assert.Single(viewModel.SupervisorPool);
    }

    // TC-WEB-MAP-002b
    [Fact]
    public void Map_WhenNoDiploma_UsesDefaultsForActionsAndProgress()
    {
        MyDiplomaViewModel viewModel = StudentDiplomaViewModelMapper.Map(MapperTestFixtures.CreateEmptyMyDiplomaDto());

        Assert.False(viewModel.HasDiploma);
        Assert.Null(viewModel.DiplomaId);
        Assert.False(viewModel.CanSelectSupervisor);
        Assert.False(viewModel.ShowSupervisorSection);
        Assert.Null(viewModel.WorkflowProgress);
        Assert.NotNull(viewModel.Documents);
        Assert.Empty(viewModel.Documents!.StudentWorkVersions);
    }

    // TC-WEB-MAP-002c
    [Fact]
    public void Map_BuildsTopicHistoryFromVersions()
    {
        MyDiplomaViewModel viewModel = StudentDiplomaViewModelMapper.Map(MapperTestFixtures.CreateMyDiplomaDto());

        Assert.Single(viewModel.TopicHistory);
        Assert.Equal("Тема роботи", viewModel.TopicHistory[0].Title);
    }
}
