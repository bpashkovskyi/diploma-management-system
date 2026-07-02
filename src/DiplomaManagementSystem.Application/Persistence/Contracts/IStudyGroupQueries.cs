namespace DiplomaManagementSystem.Application.Persistence.Contracts;

public interface IStudyGroupQueries
{
    Task<List<StudyGroupOption>> ListOptionsForSessionAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<string?> GetNameAsync(Guid studyGroupId, CancellationToken cancellationToken = default);
}
