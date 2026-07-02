using DiplomaManagementSystem.Application.Secretary.Dtos;

namespace DiplomaManagementSystem.Application.Employee.Contracts;

public interface ISupervisorDiplomaDetailsService
{
    Task<DiplomaDetailsDto?> GetDetailsAsync(
        Guid supervisorId,
        Guid diplomaId,
        CancellationToken cancellationToken = default);
}
