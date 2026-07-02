using DiplomaManagementSystem.Application.Admin.AnnualRoles.Dtos;

namespace DiplomaManagementSystem.Application.Admin.AnnualRoles.Contracts;

public interface IAnnualRoleService
{
    Task<AnnualRolesPageDto?> GetPageAsync(Guid defenceSessionId, CancellationToken cancellationToken = default);

    Task AssignAsync(AssignAnnualRoleDto request, CancellationToken cancellationToken = default);
}
