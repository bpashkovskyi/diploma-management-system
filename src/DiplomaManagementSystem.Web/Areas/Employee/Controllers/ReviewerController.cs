using DiplomaManagementSystem.Application.Constants;
using DiplomaManagementSystem.Application.Employee.Contracts;
using DiplomaManagementSystem.Application.Employee.Dtos;
using DiplomaManagementSystem.Web.Areas.Employee.Models;
using DiplomaManagementSystem.Web.Mapping;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaManagementSystem.Web.Areas.Employee.Controllers;

[Area("Employee")]
[Authorize(Roles = RoleNames.Employee)]
public sealed class ReviewerController(
    IAdmissionReviewService admissionReviewService,
    IValidator<CompleteCheckpointDto> completeCheckpointValidator) : EmployeeControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Assignments(CancellationToken cancellationToken)
    {
        IReadOnlyList<ReviewerAssignmentItemDto> items =
            await admissionReviewService.GetReviewerAssignmentsAsync(GetUserId(), cancellationToken);

        ReviewerAssignmentsViewModel model = new()
        {
            Items = items.Select(EmployeeViewModelMapper.MapReviewerAssignment).ToList(),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Complete(CompleteCheckpointViewModel model, CancellationToken cancellationToken) =>
        CompleteCheckpointWithDocumentAsync(
            model,
            completeCheckpointValidator,
            (userId, dto, document, ct) => admissionReviewService.CompleteExternalReviewAsync(userId, dto, document, ct),
            "Рецензію зафіксовано.",
            nameof(Assignments),
            cancellationToken);
}
