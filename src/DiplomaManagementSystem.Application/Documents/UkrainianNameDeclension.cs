namespace DiplomaManagementSystem.Application.Documents;

internal static class UkrainianNameDeclension
{
    public static string ToGenitiveSurname(string nominative)
    {
        if (string.IsNullOrWhiteSpace(nominative))
        {
            return string.Empty;
        }

        string name = nominative.Trim();

        if (name.EndsWith("енко", StringComparison.OrdinalIgnoreCase))
        {
            return name[..^1] + "а";
        }

        if (name.EndsWith("ський", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("цький", StringComparison.OrdinalIgnoreCase))
        {
            return name[..^2] + "ого";
        }

        if (name.EndsWith("ова", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("ева", StringComparison.OrdinalIgnoreCase))
        {
            return name[..^1] + "ої";
        }

        if (name.EndsWith('р') && name.Length <= 4)
        {
            return name + "я";
        }

        if (!name.EndsWith("а", StringComparison.OrdinalIgnoreCase))
        {
            return name + "а";
        }

        return name;
    }

    public static string ToGenitiveFirstName(string nominative)
    {
        if (string.IsNullOrWhiteSpace(nominative))
        {
            return string.Empty;
        }

        string name = nominative.Trim();

        if (name.EndsWith("ль", StringComparison.OrdinalIgnoreCase))
        {
            return name[..^1] + "я";
        }

        if (name.EndsWith("о", StringComparison.OrdinalIgnoreCase))
        {
            return name[..^1] + "а";
        }

        if (name.EndsWith("й", StringComparison.OrdinalIgnoreCase))
        {
            return name[..^1] + "я";
        }

        if (!name.EndsWith("а", StringComparison.OrdinalIgnoreCase))
        {
            return name + "а";
        }

        return name;
    }
}
