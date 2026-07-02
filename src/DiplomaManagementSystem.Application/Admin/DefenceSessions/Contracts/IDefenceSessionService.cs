using DiplomaManagementSystem.Application.Admin.DefenceSessions.Dtos;

namespace DiplomaManagementSystem.Application.Admin.DefenceSessions.Contracts;

public interface IDefenceSessionService
{
    Task<IReadOnlyList<DefenceSessionListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<DefenceSessionFormDto?> GetForEditAsync(Guid id, CancellationToken cancellationToken = default);

    Task<DefenceSessionDetailsDto?> GetDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(DefenceSessionFormDto form, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid id, DefenceSessionFormDto form, CancellationToken cancellationToken = default);

    Task ArchiveAsync(Guid id, Guid performedById, CancellationToken cancellationToken = default);
}
