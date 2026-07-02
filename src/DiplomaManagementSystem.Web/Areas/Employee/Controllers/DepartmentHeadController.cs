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
public sealed class DepartmentHeadController(
    IDepartmentHeadWorkflowService departmentHeadWorkflowService,
    IValidator<ApproveTopicDto> approveTopicValidator,
    IValidator<ReviewTopicDto> reviewTopicRejectValidator) : EmployeeControllerBase
{
    [HttpGet]
    public async Task<IActionResult> PendingTopics(CancellationToken cancellationToken)
    {
        IReadOnlyList<TopicReviewItemDto> items =
            await departmentHeadWorkflowService.GetPendingTopicsAsync(GetUserId(), cancellationToken);

        PendingTopicsViewModel model = new()
        {
            Items = items.Select(EmployeeViewModelMapper.MapTopicReviewItem).ToList(),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> ApproveTopic(ApproveTopicViewModel model, CancellationToken cancellationToken) =>
        ApproveTopicAsync(
            model,
            approveTopicValidator,
            departmentHeadWorkflowService.ApproveTopicAsync,
            "Тему затверджено.",
            nameof(PendingTopics),
            cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> RejectTopic(ReviewTopicViewModel model, CancellationToken cancellationToken) =>
        RejectTopicAsync(
            model,
            reviewTopicRejectValidator,
            departmentHeadWorkflowService.RejectTopicAsync,
            "Тему відхилено.",
            nameof(PendingTopics),
            cancellationToken);
}
