using DiplomaManagementSystem.Application.Admin.AnnualRoles.Contracts;

using DiplomaManagementSystem.Application.Admin.AnnualRoles.Dtos;

using DiplomaManagementSystem.Application.Admin.DefenceSessions.Contracts;

using DiplomaManagementSystem.Domain.Exceptions;

using DiplomaManagementSystem.Web.Areas.Admin.Models;

using DiplomaManagementSystem.Web.Extensions;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaManagementSystem.Web.Areas.Admin.Controllers;

public sealed class AnnualRolesController(

    IAnnualRoleService annualRoleService,

    IDefenceSessionService defenceSessionService,

    IValidator<AssignAnnualRoleDto> validator) : AdminControllerBase(defenceSessionService)

{

    [HttpGet]

    public async Task<IActionResult> Index(Guid defenceSessionId, CancellationToken cancellationToken)

    {

        AnnualRolesViewModel? model = await BuildIndexViewModelAsync(defenceSessionId, assignForm: null, cancellationToken);

        return model is null ? NotFound() : View(model);

    }

    [HttpPost]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Assign(AssignAnnualRoleFormViewModel model, CancellationToken cancellationToken)

    {

        AssignAnnualRoleDto dto = new(model.DefenceSessionId, model.RoleType, model.EmployeeId);

        if (!await this.TryValidateFormAsync(validator, dto, cancellationToken))

        {

            AnnualRolesViewModel? viewModel = await BuildIndexViewModelAsync(

                model.DefenceSessionId,

                model,

                cancellationToken);

            return viewModel is null ? NotFound() : View("Index", viewModel);

        }

        try

        {

            await annualRoleService.AssignAsync(dto, cancellationToken);

            AnnualRolesViewModel? viewModel = await BuildIndexViewModelAsync(

                model.DefenceSessionId,

                null,

                cancellationToken);

            if (viewModel is null)

            {

                return NotFound();

            }

            ViewData["Success"] = "Роль призначено.";

            return View("Index", viewModel);

        }

        catch (DomainException ex)

        {

            ModelState.AddModelError(string.Empty, ex.Message);

            AnnualRolesViewModel? viewModel = await BuildIndexViewModelAsync(

                model.DefenceSessionId,

                model,

                cancellationToken);

            return viewModel is null ? NotFound() : View("Index", viewModel);

        }

    }

    private async Task<AnnualRolesViewModel?> BuildIndexViewModelAsync(

        Guid defenceSessionId,

        AssignAnnualRoleFormViewModel? assignForm,

        CancellationToken cancellationToken)

    {

        AnnualRolesPageDto? page = await annualRoleService.GetPageAsync(defenceSessionId, cancellationToken);

        if (page is null)

        {

            return null;

        }

        AnnualRolesViewModel model = new()

        {

            DefenceSessionId = page.DefenceSessionId,

            SessionLabel = page.SessionLabel,

            Employees = page.Employees

                .Select(employee => new SelectListItem($"{employee.FullName} ({employee.Email})", employee.Id.ToString()))

                .ToList(),

            Roles = page.Roles.Select(role => new AnnualRoleSlotViewModel

            {

                RoleType = role.RoleType,

                RoleDisplay = UkrainianDisplay.FormatAnnualRoleType(role.RoleType),

                AssignedEmployeeId = role.AssignedEmployeeId,

                AssignedEmployeeName = role.AssignedEmployeeName,

                SelectedEmployeeId = role.AssignedEmployeeId ?? Guid.Empty,

            }).ToList(),

        };

        if (assignForm is not null)

        {

            AnnualRoleSlotViewModel? slot = model.Roles.FirstOrDefault(role => role.RoleType == assignForm.RoleType);

            if (slot is not null)

            {

                slot.SelectedEmployeeId = assignForm.EmployeeId;

            }

        }

        return model;

    }

}

