using DiplomaManagementSystem.Application.Admin.Students.Dtos;

namespace DiplomaManagementSystem.Application.Admin.Students.Contracts;

public interface IStudentAdminService
{
    Task<IReadOnlyList<StudentListItemDto>> GetAllAsync(
        Guid? defenceSessionId = null,
        CancellationToken cancellationToken = default);

    Task<StudentFormDto?> GetForEditAsync(Guid id, CancellationToken cancellationToken = default);

    Task<StudentDetailsDto?> GetDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentSessionOptionDto>> GetSessionOptionsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentGroupOptionDto>> GetGroupOptionsAsync(
        Guid defenceSessionId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(StudentFormDto dto, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid id, StudentFormDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
