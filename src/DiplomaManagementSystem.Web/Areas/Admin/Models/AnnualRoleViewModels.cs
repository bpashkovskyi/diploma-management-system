using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Web.Areas.Admin.Models;

public sealed class AnnualRolesViewModel
{
    public Guid DefenceSessionId { get; set; }

    public string SessionLabel { get; set; } = string.Empty;

    public IReadOnlyList<AnnualRoleSlotViewModel> Roles { get; set; } = [];

    public IReadOnlyList<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Employees { get; set; } = [];
}

public sealed class AnnualRoleSlotViewModel
{
    public AnnualRoleType RoleType { get; set; }

    public string RoleDisplay { get; set; } = string.Empty;

    public Guid? AssignedEmployeeId { get; set; }

    public string? AssignedEmployeeName { get; set; }

    public Guid SelectedEmployeeId { get; set; }
}

public sealed class AssignAnnualRoleFormViewModel
{
    public Guid DefenceSessionId { get; set; }

    public AnnualRoleType RoleType { get; set; }

    public Guid EmployeeId { get; set; }
}
