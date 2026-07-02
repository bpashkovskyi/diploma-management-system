using DiplomaManagementSystem.Application.Admin.Employees.Dtos;

namespace DiplomaManagementSystem.Application.Admin.Employees.Contracts;

public interface IEmployeeAdminService
{
    Task<IReadOnlyList<EmployeeListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<EmployeeFormDto?> GetForEditAsync(Guid id, CancellationToken cancellationToken = default);

    Task<EmployeeDetailsDto?> GetDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(EmployeeFormDto dto, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid id, EmployeeFormDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
