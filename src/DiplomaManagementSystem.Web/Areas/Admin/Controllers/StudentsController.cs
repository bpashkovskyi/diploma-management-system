using DiplomaManagementSystem.Application.Admin.DefenceSessions.Contracts;
using DiplomaManagementSystem.Application.Admin.DefenceSessions.Dtos;
using DiplomaManagementSystem.Application.Admin.Students.Contracts;
using DiplomaManagementSystem.Application.Admin.Students.Dtos;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;
using DiplomaManagementSystem.Web.Areas.Admin.Models;
using DiplomaManagementSystem.Web.Extensions;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaManagementSystem.Web.Areas.Admin.Controllers;

public sealed class StudentsController(
    IStudentAdminService studentAdminService,
    IDefenceSessionService defenceSessionService,
    IValidator<StudentFormDto> validator) : AdminControllerBase(defenceSessionService)
{
    [HttpGet]
    public async Task<IActionResult> Index(Guid? defenceSessionId, CancellationToken cancellationToken)
    {
        if (defenceSessionId is null || defenceSessionId == Guid.Empty)
        {
            AdminFlashMessages.SetInfo(this, "Оберіть сесію захисту, щоб керувати студентами.");
            return RedirectToAction("Index", "DefenceSessions");
        }

        string? sessionLabel = await GetSessionLabelAsync(defenceSessionId.Value, cancellationToken);
        if (sessionLabel is null)
        {
            return NotFound();
        }

        IReadOnlyList<StudentListItemDto> items = await studentAdminService.GetAllAsync(defenceSessionId.Value, cancellationToken);

        StudentListViewModel model = new()
        {
            DefenceSessionId = defenceSessionId.Value,
            SessionLabel = sessionLabel,
            Items = items.Select(MapListItem).ToList(),
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid defenceSessionId, CancellationToken cancellationToken)
    {
        StudentFormViewModel? model = await BuildFormViewModelAsync(null, defenceSessionId, cancellationToken);
        return model is null ? NotFound() : View("Form", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentFormViewModel model, CancellationToken cancellationToken)
    {
        StudentFormDto dto = ToDto(model);
        if (!await this.TryValidateFormAsync(validator, dto, cancellationToken))
        {
            return View("Form", await RebuildFormViewModelAsync(model, cancellationToken) ?? model);
        }

        try
        {
            await studentAdminService.CreateAsync(dto, cancellationToken);
            return RedirectToStudentList(model.DefenceSessionId);
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Form", await RebuildFormViewModelAsync(model, cancellationToken) ?? model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        StudentFormDto? dto = await studentAdminService.GetForEditAsync(id, cancellationToken);
        if (dto is null)
        {
            return NotFound();
        }

        StudentFormViewModel? model = await BuildFormViewModelAsync(dto, dto.DefenceSessionId, cancellationToken);
        return model is null ? NotFound() : View("Form", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, StudentFormViewModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        StudentFormDto dto = ToDto(model);
        if (!await this.TryValidateFormAsync(validator, dto, cancellationToken))
        {
            return View("Form", await RebuildFormViewModelAsync(model, cancellationToken) ?? model);
        }

        try
        {
            await studentAdminService.UpdateAsync(id, dto, cancellationToken);
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Form", await RebuildFormViewModelAsync(model, cancellationToken) ?? model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        StudentDetailsDto? details = await studentAdminService.GetDetailsAsync(id, cancellationToken);
        if (details is null)
        {
            return NotFound();
        }

        return View(new StudentDetailsViewModel
        {
            Id = details.Id,
            DefenceSessionId = details.DefenceSessionId,
            FullName = details.FullName,
            Email = details.Email,
            SessionLabel = details.SessionLabel,
            SessionType = details.SessionType,
            StudyGroupName = details.StudyGroupName,
            HasDiploma = details.HasDiploma,
            CreatedAt = details.CreatedAt,
        });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        StudentDeleteViewModel? model = await BuildDeleteViewModelAsync(id, cancellationToken);
        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        StudentDetailsDto? details = await studentAdminService.GetDetailsAsync(id, cancellationToken);
        try
        {
            await studentAdminService.DeleteAsync(id, cancellationToken);
            return RedirectToStudentList(details?.DefenceSessionId ?? Guid.Empty);
        }
        catch (DomainException ex)
        {
            StudentDeleteViewModel? model = await BuildDeleteViewModelAsync(id, cancellationToken);
            if (model is null)
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    private async Task<StudentDeleteViewModel?> BuildDeleteViewModelAsync(Guid id, CancellationToken cancellationToken)
    {
        StudentDetailsDto? details = await studentAdminService.GetDetailsAsync(id, cancellationToken);

        return details is null
            ? null
            : new StudentDeleteViewModel
            {
                Id = details.Id,
                DefenceSessionId = details.DefenceSessionId,
                FullName = details.FullName,
                Email = details.Email,
                SessionType = details.SessionType,
                HasDiploma = details.HasDiploma,
            };
    }

    private async Task<StudentFormViewModel?> BuildFormViewModelAsync(
        StudentFormDto? dto,
        Guid defenceSessionId,
        CancellationToken cancellationToken)
    {
        string? sessionLabel = dto?.SessionLabel;
        DefenceSessionType? sessionType = dto?.SessionType;

        if (sessionLabel is null)
        {
            sessionLabel = await GetSessionLabelAsync(defenceSessionId, cancellationToken);
            if (sessionLabel is null)
            {
                return null;
            }

            DefenceSessionFormDto? session = await DefenceSessionService.GetForEditAsync(defenceSessionId, cancellationToken);
            sessionType = session?.Type;
        }

        StudentFormViewModel model = new()
        {
            Id = dto?.Id,
            FullName = dto?.FullName ?? string.Empty,
            Email = dto?.Email ?? string.Empty,
            DefenceSessionId = defenceSessionId,
            SessionLabel = sessionLabel,
            StudyGroupId = dto?.StudyGroupId ?? Guid.Empty,
            SessionType = sessionType,
        };

        await PopulateStudyGroupsAsync(model, cancellationToken);
        return model;
    }

    private async Task<StudentFormViewModel?> RebuildFormViewModelAsync(
        StudentFormViewModel model,
        CancellationToken cancellationToken)
    {
        string? sessionLabel = await GetSessionLabelAsync(model.DefenceSessionId, cancellationToken);
        if (sessionLabel is null)
        {
            return null;
        }

        DefenceSessionFormDto? session = await DefenceSessionService.GetForEditAsync(model.DefenceSessionId, cancellationToken);
        model.SessionLabel = sessionLabel;
        model.SessionType = session?.Type;
        await PopulateStudyGroupsAsync(model, cancellationToken);
        return model;
    }

    private async Task PopulateStudyGroupsAsync(StudentFormViewModel model, CancellationToken cancellationToken)
    {
        IReadOnlyList<StudentGroupOptionDto> groups = await studentAdminService.GetGroupOptionsAsync(
            model.DefenceSessionId,
            cancellationToken);

        model.StudyGroups = groups
            .Select(group => new SelectListItem(group.Name, group.Id.ToString(), group.Id == model.StudyGroupId))
            .ToList();
    }

    private RedirectToActionResult RedirectToStudentList(Guid defenceSessionId) =>
        RedirectToAction(nameof(Index), new { defenceSessionId });

    private static StudentListItemViewModel MapListItem(StudentListItemDto item) =>
        new()
        {
            Id = item.Id,
            FullName = item.FullName,
            Email = item.Email,
            StudyGroupName = item.StudyGroupName,
            CreatedAt = item.CreatedAt,
        };

    private static StudentFormDto ToDto(StudentFormViewModel model) =>
        new(model.Id, model.FullName, model.Email, model.DefenceSessionId, model.StudyGroupId);
}
