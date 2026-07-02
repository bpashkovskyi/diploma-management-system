using DiplomaManagementSystem.Application.Common;
using DiplomaManagementSystem.Application.Identity;
using DiplomaManagementSystem.Application.Secretary.Dtos;
using DiplomaManagementSystem.Domain;
using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Secretary;

internal static class SecretaryDiplomaListProjection
{
    private const string MissingLabel = "—";
    private static readonly int OutcomeStepTotal = AdmissionStepSequence.OutcomeSteps.Count;

    public static DiplomaListItemDto MapListItem(
        Diploma diploma,
        IReadOnlyDictionary<Guid, ApplicationUser> users,
        IReadOnlyDictionary<Guid, string> studyGroupNames)
    {
        users.TryGetValue(diploma.StudentId, out ApplicationUser? student);

        string? supervisorName = null;
        if (diploma.SupervisorId.HasValue
            && users.TryGetValue(diploma.SupervisorId.Value, out ApplicationUser? supervisor))
        {
            supervisorName = supervisor.FullName;
        }

        string studyGroupName = MissingLabel;
        if (student?.StudyGroupId is Guid groupId
            && studyGroupNames.TryGetValue(groupId, out string? name))
        {
            studyGroupName = name;
        }

        int completed = AdmissionStepSequence.OutcomeSteps
            .Count(step => AdmissionStepStatusResolver.HasPassingAttempt(
                step,
                diploma.AdmissionStepAttempts));

        string? topicTitle = diploma.TopicVersions
            .OrderByDescending(version => version.VersionNumber)
            .Select(version => version.Title)
            .FirstOrDefault();

        return new DiplomaListItemDto(
            diploma.Id,
            student?.FullName ?? MissingLabel,
            student?.Email ?? string.Empty,
            studyGroupName,
            supervisorName,
            topicTitle,
            diploma.LifecycleStatus,
            diploma.AdmissionStatus,
            diploma.CurrentAdmissionStep,
            completed,
            OutcomeStepTotal);
    }

    public static List<DiplomaListItemDto> MapListItems(
        IEnumerable<Diploma> diplomas,
        IReadOnlyDictionary<Guid, ApplicationUser> users,
        IReadOnlyDictionary<Guid, string> studyGroupNames) =>
        diplomas
            .Select(diploma => MapListItem(diploma, users, studyGroupNames))
            .OrderBy(item => PersonNameSort.SurnameKey(item.StudentFullName), StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(item => item.StudentFullName, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
}
