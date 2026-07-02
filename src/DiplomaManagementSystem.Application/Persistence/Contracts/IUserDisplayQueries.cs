using DiplomaManagementSystem.Application.Identity;

namespace DiplomaManagementSystem.Application.Persistence.Contracts;

public interface IUserDisplayQueries
{
    Task<Dictionary<Guid, ApplicationUser>> LoadUsersAsync(
        IReadOnlyCollection<Guid> userIds,
        CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, string>> LoadFullNamesAsync(
        IReadOnlyCollection<Guid> userIds,
        CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, string>> LoadStudyGroupNamesAsync(
        IReadOnlyCollection<Guid> studyGroupIds,
        CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, StudentDisplayInfo>> LoadStudentDisplaysAsync(
        IReadOnlyCollection<Guid> studentIds,
        CancellationToken cancellationToken = default);

    Task<List<UserOption>> LoadEmployeeOptionsAsync(CancellationToken cancellationToken = default);

    Task<bool> IsEmployeeAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<StudentStorageContext?> GetStudentStorageContextAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);
}
