using DiplomaManagementSystem.Web.Areas.Secretary.Models;
using DiplomaManagementSystem.Web.Models.Shared;

namespace DiplomaManagementSystem.Web.Mapping;

internal static class TopicHistoryMapper
{
    public static IReadOnlyList<TopicHistoryEntryViewModel> Map(IReadOnlyList<TopicVersionItemViewModel> versions) =>
        versions
            .Select(version => new TopicHistoryEntryViewModel
            {
                VersionNumber = version.VersionNumber,
                Title = version.Title,
                StatusDisplay = version.StatusDisplay,
                SubmittedAt = version.SubmittedAt,
                RejectionReason = version.RejectionReason,
            })
            .ToList();

    public static IReadOnlyList<TopicHistoryEntryViewModel> Map(IReadOnlyList<TopicVersionDetailViewModel> versions) =>
        versions
            .Select(version => new TopicHistoryEntryViewModel
            {
                VersionNumber = version.VersionNumber,
                Title = version.Title,
                StatusDisplay = version.StatusDisplay,
                SubmittedAtDisplay = version.SubmittedAtDisplay,
                SupervisorApprovalLine = version.SupervisorApprovalLine,
                HeadApprovalLine = version.HeadApprovalLine,
                RejectionLine = version.RejectionLine,
                RejectionReason = version.RejectionReason,
            })
            .ToList();
}
