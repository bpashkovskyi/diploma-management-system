using System.Security.Claims;

namespace DiplomaManagementSystem.Web.AdminPreview;

public interface IAdminPreviewService
{
    AdminPreviewMode GetMode(HttpContext httpContext, ClaimsPrincipal? user = null);

    void SetMode(HttpContext httpContext, AdminPreviewMode mode);

    Guid? GetImpersonatedUserId(HttpContext httpContext, ClaimsPrincipal? user = null);

    void SetImpersonatedUserId(HttpContext httpContext, Guid userId);

    void ClearImpersonation(HttpContext httpContext);

    bool RequiresImpersonation(AdminPreviewMode mode);

    bool HasImpersonation(HttpContext httpContext, ClaimsPrincipal? user = null);

    bool IsAdmin(ClaimsPrincipal user, HttpContext? httpContext = null);

    bool IsActivePreview(HttpContext httpContext);

    string GetModeDisplayName(AdminPreviewMode mode);
}
