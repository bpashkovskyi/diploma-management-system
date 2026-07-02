using DiplomaManagementSystem.Application.Admin.DefenceSessions.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace DiplomaManagementSystem.Web.Areas.Admin.Controllers;

public sealed class HomeController(IDefenceSessionService defenceSessionService) : AdminControllerBase(defenceSessionService)
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
