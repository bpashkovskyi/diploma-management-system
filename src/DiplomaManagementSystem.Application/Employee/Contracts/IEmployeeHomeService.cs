using DiplomaManagementSystem.Application.Employee.Dtos;

namespace DiplomaManagementSystem.Application.Employee.Contracts;

public interface IEmployeeHomeService
{
    Task<EmployeeHomeDto> GetHomeAsync(Guid employeeId, CancellationToken cancellationToken = default);
}
