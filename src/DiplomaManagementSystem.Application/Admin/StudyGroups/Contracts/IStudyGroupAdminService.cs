using DiplomaManagementSystem.Application.Admin.StudyGroups.Dtos;

namespace DiplomaManagementSystem.Application.Admin.StudyGroups.Contracts;

public interface IStudyGroupAdminService
{
    Task<IReadOnlyList<StudyGroupListItemDto>> GetAllAsync(
        Guid defenceSessionId,
        CancellationToken cancellationToken = default);

    Task<StudyGroupFormDto?> GetForEditAsync(Guid id, CancellationToken cancellationToken = default);

    Task<StudyGroupListItemDto?> GetListItemAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(StudyGroupFormDto dto, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid id, StudyGroupFormDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
