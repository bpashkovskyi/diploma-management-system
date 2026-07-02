using DiplomaManagementSystem.Application.Secretary.Dtos;
using DiplomaManagementSystem.Web.Areas.Secretary.Models;

namespace DiplomaManagementSystem.Web.Mapping;

internal static class SecretaryReportsViewModelMapper
{
    public static AdmittedReportViewModel Map(AdmittedReportDto report) => new()
    {
        SessionId = report.SessionId,
        SessionLabel = report.SessionLabel,
        Items = report.Items.Select(MapItem).ToList(),
    };

    private static AdmittedReportItemViewModel MapItem(AdmittedReportItemDto item) => new()
    {
        StudentFullName = item.StudentFullName,
        StudyGroupName = item.StudyGroupName,
        TopicTitle = item.TopicTitle,
        SupervisorName = item.SupervisorName,
        ReviewerName = item.ReviewerName,
        DefenceDate = item.DefenceDate,
    };
}
