namespace DiplomaManagementSystem.Web.AdminPreview;

internal static class AdminPreviewReturnUrl
{
    public static string Build(HttpContext httpContext)
    {
        string pathBase = httpContext.Request.PathBase.Value ?? string.Empty;
        string path = httpContext.Request.Path.Value ?? "/";
        string query = httpContext.Request.QueryString.Value ?? string.Empty;
        return $"{pathBase}{path}{query}";
    }
}
