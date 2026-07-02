namespace DiplomaManagementSystem.Application.Documents;

internal static class PersonNameFileSegment
{
    public static string FormatGenitive(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return "Nevidomyi";
        }

        (string surname, string firstName) = ParseSurnameFirst(fullName);
        string surnameSegment = UkrainianTransliteration.ToFileSegment(UkrainianNameDeclension.ToGenitiveSurname(surname));
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return surnameSegment;
        }

        string firstNameSegment = UkrainianTransliteration.ToFileSegment(
            UkrainianNameDeclension.ToGenitiveFirstName(firstName));

        return $"{surnameSegment}_{firstNameSegment}";
    }

    private static (string Surname, string FirstName) ParseSurnameFirst(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return ("Nevidomyi", string.Empty);
        }

        string[] parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Length switch
        {
            >= 2 => (parts[0], parts[1]),
            1 => (parts[0], string.Empty),
            _ => ("Nevidomyi", string.Empty),
        };
    }
}
