using DiplomaManagementSystem.Application.Employee.Dtos;
using DiplomaManagementSystem.Application.Secretary.Dtos;

namespace DiplomaManagementSystem.Application.Employee.Contracts;

public interface ISupervisorDiplomaListService
{
    Task<SupervisorDiplomaListPageDto> GetListAsync(
        Guid supervisorId,
        DiplomaListFilterDto filter,
        CancellationToken cancellationToken = default);
}
