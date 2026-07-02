using Microsoft.AspNetCore.Mvc;

namespace DiplomaManagementSystem.Web.Areas.Admin;

/// <summary>
/// Flash messages for POST-redirect-GET actions. Form re-renders use <see cref="ModelStateDictionary"/> instead.
/// </summary>
internal static class AdminFlashMessages
{
    public static void SetSuccess(Controller controller, string message) =>
        controller.TempData["Success"] = message;

    public static void SetError(Controller controller, string message) =>
        controller.TempData["Error"] = message;

    public static void SetInfo(Controller controller, string message) =>
        controller.TempData["Info"] = message;
}
