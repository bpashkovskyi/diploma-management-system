namespace DiplomaManagementSystem.Web.AdminPreview;

internal static class AdminPreviewModeRules
{
    /// <summary>Legacy session value for the removed Secretary preview mode (maps to <see cref="AdminPreviewMode.Employee"/>).</summary>
    public const int LegacyEmployeeStoredValue = 1;

    public static AdminPreviewMode FromStoredValue(int stored) => stored switch
    {
        LegacyEmployeeStoredValue => AdminPreviewMode.Employee,
        int value when Enum.IsDefined(typeof(AdminPreviewMode), value) => (AdminPreviewMode)value,
        _ => AdminPreviewMode.Admin,
    };

    public static AdminPreviewMode Normalize(AdminPreviewMode mode) =>
        (int)mode == LegacyEmployeeStoredValue ? AdminPreviewMode.Employee : mode;

    public static bool IsEmployeePreviewMode(AdminPreviewMode mode) =>
        Normalize(mode) == AdminPreviewMode.Employee;

    public static bool IsEmployeeArea(string? area) =>
        area is "Employee" or "Secretary";

    public static bool AreaMatchesMode(string area, AdminPreviewMode mode) => mode switch
    {
        AdminPreviewMode.Admin => area == "Admin",
        AdminPreviewMode.Student => area == "Student",
        _ when IsEmployeePreviewMode(mode) => IsEmployeeArea(area),
        _ => false,
    };

    public static bool IsValidReturnUrlArea(string? area, AdminPreviewMode mode) => mode switch
    {
        AdminPreviewMode.Admin => area == "Admin",
        AdminPreviewMode.Student => area == "Student",
        _ when IsEmployeePreviewMode(mode) => IsEmployeeArea(area),
        _ => false,
    };

    public static bool IsEmployeeSurface(AdminPreviewMode mode) => IsEmployeePreviewMode(mode);
}
