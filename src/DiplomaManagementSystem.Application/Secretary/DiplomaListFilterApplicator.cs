using DiplomaManagementSystem.Application.Identity;
using DiplomaManagementSystem.Application.Secretary.Dtos;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Secretary;

internal static class DiplomaListFilterApplicator
{
    public static IEnumerable<Diploma> Apply(
        IEnumerable<Diploma> diplomas,
        DiplomaListFilterDto filter,
        IReadOnlyDictionary<Guid, ApplicationUser> users)
    {
        IEnumerable<Diploma> filtered = diplomas;

        if (filter.LifecycleStatus.HasValue)
        {
            filtered = filtered.Where(diploma => diploma.LifecycleStatus == filter.LifecycleStatus.Value);
        }

        if (filter.CurrentAdmissionStep.HasValue)
        {
            filtered = filtered.Where(diploma =>
                diploma.LifecycleStatus == DiplomaLifecycleStatus.DocumentsInProgress
                && diploma.CurrentAdmissionStep == filter.CurrentAdmissionStep.Value);
        }

        if (filter.SupervisorAssignmentStatus.HasValue)
        {
            filtered = filtered.Where(diploma =>
                diploma.SupervisorAssignmentStatus == filter.SupervisorAssignmentStatus.Value);
        }

        if (filter.AdmissionStatus.HasValue)
        {
            filtered = filtered.Where(diploma => diploma.AdmissionStatus == filter.AdmissionStatus.Value);
        }

        if (filter.StudyGroupId.HasValue)
        {
            filtered = filtered.Where(diploma =>
            {
                users.TryGetValue(diploma.StudentId, out ApplicationUser? student);
                return student?.StudyGroupId == filter.StudyGroupId;
            });
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            string term = filter.Search.Trim();
            filtered = filtered.Where(diploma =>
            {
                if (!users.TryGetValue(diploma.StudentId, out ApplicationUser? student))
                {
                    return false;
                }

                return student.FullName.Contains(term, StringComparison.OrdinalIgnoreCase)
                       || (student.Email?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false);
            });
        }

        return filtered;
    }
}
