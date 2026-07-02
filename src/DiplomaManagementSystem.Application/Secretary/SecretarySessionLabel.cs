using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Secretary;

public static class SecretarySessionLabel
{
    public static string Format(int year, DefenceSessionType type, int? semester)
    {
        string typeLabel = type switch
        {
            DefenceSessionType.Bachelor => "Бакалавр",
            DefenceSessionType.Master => "Магістр",
            _ => type.ToString(),
        };

        string label = $"{year} — {typeLabel}";
        return semester.HasValue ? $"{label} (сем. {semester})" : label;
    }
}
