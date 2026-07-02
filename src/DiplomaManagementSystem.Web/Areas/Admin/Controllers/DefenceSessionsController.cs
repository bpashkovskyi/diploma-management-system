using System.Security.Claims;

using DiplomaManagementSystem.Application.Admin.DefenceSessions.Contracts;
using DiplomaManagementSystem.Application.Admin.DefenceSessions.Dtos;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;
using DiplomaManagementSystem.Web.Areas.Admin.Models;
using DiplomaManagementSystem.Web.Extensions;
using DiplomaManagementSystem.Web.Mapping;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace DiplomaManagementSystem.Web.Areas.Admin.Controllers;

public sealed class DefenceSessionsController(
    IDefenceSessionService defenceSessionService,
    IValidator<DefenceSessionFormDto> validator) : AdminControllerBase(defenceSessionService)
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        IReadOnlyList<DefenceSessionListItemDto> items = await DefenceSessionService.GetAllAsync(cancellationToken);
        DefenceSessionListViewModel model = new()
        {
            Items = items.Select(AdminDefenceSessionViewModelMapper.MapListItem).ToList(),
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("Form", BuildFormViewModel(null));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DefenceSessionFormViewModel model, CancellationToken cancellationToken)
    {
        DefenceSessionFormDto dto = ToDto(model);
        if (!await this.TryValidateFormAsync(validator, dto, cancellationToken))
        {
            return View("Form", model);
        }

        try
        {
            await DefenceSessionService.CreateAsync(dto, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        DefenceSessionFormDto? dto = await DefenceSessionService.GetForEditAsync(id, cancellationToken);
        if (dto is null)
        {
            return NotFound();
        }

        return View("Form", BuildFormViewModel(dto));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, DefenceSessionFormViewModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        DefenceSessionFormDto dto = ToDto(model);
        if (!await this.TryValidateFormAsync(validator, dto, cancellationToken))
        {
            return View("Form", model);
        }

        try
        {
            await DefenceSessionService.UpdateAsync(id, dto, cancellationToken);
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        DefenceSessionDetailsDto? details = await DefenceSessionService.GetDetailsAsync(id, cancellationToken);
        if (details is null)
        {
            return NotFound();
        }

        return View(AdminDefenceSessionViewModelMapper.MapDetails(details));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Archive(Guid id, bool returnToList = false, CancellationToken cancellationToken = default)
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdValue is null || !Guid.TryParse(userIdValue, out Guid userId))
        {
            return Challenge();
        }

        try
        {
            await DefenceSessionService.ArchiveAsync(id, userId, cancellationToken);
            AdminFlashMessages.SetSuccess(this, "Сесію заархівовано.");
        }
        catch (DomainException ex)
        {
            AdminFlashMessages.SetError(this, ex.Message);
        }

        return returnToList
            ? RedirectToAction(nameof(Index))
            : RedirectToAction(nameof(Details), new { id });
    }

    private static DefenceSessionFormViewModel BuildFormViewModel(DefenceSessionFormDto? dto)
    {
        if (dto is null)
        {
            return new DefenceSessionFormViewModel
            {
                Year = DateTime.UtcNow.Year,
                Type = DefenceSessionType.Bachelor,
            };
        }

        return new DefenceSessionFormViewModel
        {
            Id = dto.Id,
            Year = dto.Year,
            Type = dto.Type,
            Semester = dto.Semester,
        };
    }

    private static DefenceSessionFormDto ToDto(DefenceSessionFormViewModel model)
    {
        return new DefenceSessionFormDto(model.Id, model.Year, model.Type, model.Semester);
    }
}
