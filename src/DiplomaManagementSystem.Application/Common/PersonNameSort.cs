namespace DiplomaManagementSystem.Application.Common;

public static class PersonNameSort
{
    public static string SurnameKey(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return string.Empty;
        }

        string[] parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[^1] : fullName.Trim();
    }

    public static int CompareBySurname(string? left, string? right)
    {
        int surnameCompare = string.Compare(
            SurnameKey(left),
            SurnameKey(right),
            StringComparison.CurrentCultureIgnoreCase);

        if (surnameCompare != 0)
        {
            return surnameCompare;
        }

        return string.Compare(left, right, StringComparison.CurrentCultureIgnoreCase);
    }
}
