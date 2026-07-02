using DiplomaManagementSystem.Application.Secretary.Dtos;

namespace DiplomaManagementSystem.Application.Secretary.Contracts;

public interface ISecretaryDiplomaListService
{
    Task<DiplomaListPageDto?> GetListAsync(
        Guid sessionId,
        DiplomaListFilterDto filter,
        CancellationToken cancellationToken = default);
}
