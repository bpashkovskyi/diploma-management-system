using DiplomaManagementSystem.Application.AdminPreview.Contracts;
using DiplomaManagementSystem.Application.Constants;
using DiplomaManagementSystem.Application.Secretary.Contracts;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Web.AdminPreview;
using DiplomaManagementSystem.Web.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaManagementSystem.Web.Controllers;

[Authorize(Roles = RoleNames.Admin)]
public sealed class AdminPreviewController(
    IAdminPreviewService adminPreviewService,
    IAdminPreviewUserPickerService userPickerService,
    IAdminPreviewUserLookup adminPreviewUserLookup,
    ISecretaryAccessService secretaryAccessService) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Set(
        AdminPreviewMode mode,
        string? returnUrl = null,
        CancellationToken cancellationToken = default)
    {
        AdminPreviewMode normalizedMode = AdminPreviewModeRules.Normalize(mode);
        adminPreviewService.SetMode(HttpContext, normalizedMode);
        string? safeReturnUrl = ResolveReturnUrl(returnUrl, normalizedMode);

        if (adminPreviewService.RequiresImpersonation(normalizedMode))
        {
            return RedirectToAction(nameof(SelectUser), new { mode = normalizedMode, returnUrl = safeReturnUrl });
        }

        return await RedirectToLocalAsync(safeReturnUrl, normalizedMode, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> SelectUser(
        AdminPreviewMode mode,
        string? returnUrl = null,
        CancellationToken cancellationToken = default)
    {
        AdminPreviewMode normalizedMode = AdminPreviewModeRules.Normalize(mode);
        if (!adminPreviewService.RequiresImpersonation(normalizedMode))
        {
            return RedirectToAction("Index", "Home");
        }

        AdminPreviewMode currentMode = adminPreviewService.GetMode(HttpContext);
        if (currentMode != normalizedMode)
        {
            adminPreviewService.SetMode(HttpContext, normalizedMode);
        }

        UserKind userKind = normalizedMode == AdminPreviewMode.Student ? UserKind.Student : UserKind.Employee;
        IReadOnlyList<AdminPreviewUserOption> users = await userPickerService.GetUsersAsync(userKind, cancellationToken);

        AdminPreviewSelectUserViewModel model = new()
        {
            Mode = normalizedMode,
            ModeDisplay = adminPreviewService.GetModeDisplayName(normalizedMode),
            ReturnUrl = returnUrl ?? Url.Content("~/"),
            Users = users
                .Select(user => new AdminPreviewUserListItemViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Subtitle = user.Subtitle,
                })
                .ToList(),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetUser(
        Guid userId,
        string? returnUrl = null,
        CancellationToken cancellationToken = default)
    {
        AdminPreviewMode mode = adminPreviewService.GetMode(HttpContext);
        if (!adminPreviewService.RequiresImpersonation(mode))
        {
            return RedirectToAction("Index", "Home");
        }

        UserKind expectedKind = mode == AdminPreviewMode.Student ? UserKind.Student : UserKind.Employee;
        AdminPreviewUserProfile? user = await adminPreviewUserLookup.FindAsync(userId, cancellationToken);

        if (user is null || user.UserKind != expectedKind)
        {
            TempData["Error"] = "Обрано недійсного користувача для цього режиму перегляду.";
            return RedirectToAction(nameof(SelectUser), new { mode, returnUrl });
        }

        adminPreviewService.SetImpersonatedUserId(HttpContext, userId);
        return await RedirectToLocalAsync(ResolveReturnUrl(returnUrl, mode), mode, cancellationToken);
    }

    private string? ResolveReturnUrl(string? returnUrl, AdminPreviewMode mode)
    {
        if (returnUrl is null || !Url.IsLocalUrl(returnUrl))
        {
            return null;
        }

        return AdminPreviewRedirectRules.IsReturnUrlValidForMode(returnUrl, mode)
            ? returnUrl
            : null;
    }

    private async Task<IActionResult> RedirectToLocalAsync(
        string? returnUrl,
        AdminPreviewMode? mode = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(returnUrl))
        {
            return Redirect(returnUrl);
        }

        AdminPreviewMode effectiveMode = mode ?? adminPreviewService.GetMode(HttpContext);
        if (AdminPreviewModeRules.IsEmployeeSurface(effectiveMode)
            && adminPreviewService.GetImpersonatedUserId(HttpContext) is Guid employeeId
            && await secretaryAccessService.IsSecretaryAsync(employeeId, cancellationToken))
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Secretary" });
        }

        return AdminPreviewModeRules.Normalize(effectiveMode) switch
        {
            AdminPreviewMode.Student => RedirectToAction("Index", "Diploma", new { area = "Student" }),
            AdminPreviewMode.Employee => RedirectToAction("Index", "Home", new { area = "Employee" }),
            _ => RedirectToAction("Index", "Home", new { area = "Admin" }),
        };
    }
}
