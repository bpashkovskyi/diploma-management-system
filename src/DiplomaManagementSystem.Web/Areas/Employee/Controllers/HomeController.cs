using System.Security.Claims;
using DiplomaManagementSystem.Application.Constants;
using DiplomaManagementSystem.Application.Employee.Contracts;
using DiplomaManagementSystem.Web.Areas.Employee.Models;
using DiplomaManagementSystem.Web.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaManagementSystem.Web.Areas.Employee.Controllers;

[Area("Employee")]
[Authorize(Roles = RoleNames.Employee)]
public sealed class HomeController(IEmployeeHomeService employeeHomeService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        EmployeeHomeViewModel model = new()
        {
            Roles = (await employeeHomeService.GetHomeAsync(GetUserId(), cancellationToken))
                .Roles
                .Select(EmployeeViewModelMapper.MapRoleCard)
                .ToList(),
        };

        return View(model);
    }

    private Guid GetUserId()
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdValue is null || !Guid.TryParse(userIdValue, out Guid userId))
        {
            throw new InvalidOperationException("Authenticated user id is missing.");
        }

        return userId;
    }
}
