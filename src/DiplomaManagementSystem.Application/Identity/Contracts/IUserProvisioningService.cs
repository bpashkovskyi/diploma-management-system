using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Identity.Contracts;

public interface IUserProvisioningService
{
    Task<ApplicationUser> CreateStudentAsync(
        string fullName,
        string email,
        Guid defenceSessionId,
        Guid studyGroupId,
        CancellationToken cancellationToken = default);

    Task<ApplicationUser> CreateEmployeeAsync(
        string fullName,
        string email,
        CancellationToken cancellationToken = default);

    Task<StudyGroup> GetOrCreateStudyGroupByNameAsync(
        Guid defenceSessionId,
        string name,
        CancellationToken cancellationToken = default);

    Task EnsureEmailAvailableAsync(
        string email,
        Guid? excludeUserId = null,
        CancellationToken cancellationToken = default);
}
