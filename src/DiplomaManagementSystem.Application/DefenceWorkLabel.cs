using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application;

public static class DefenceWorkLabel
{
    public static string Singular(DefenceSessionType type) => type switch
    {
        DefenceSessionType.Bachelor => "бакалаврська робота",
        DefenceSessionType.Master => "магістерська робота",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string SingularCapitalized(DefenceSessionType type) => type switch
    {
        DefenceSessionType.Bachelor => "Бакалаврська робота",
        DefenceSessionType.Master => "Магістерська робота",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string SingularAccusative(DefenceSessionType type) => type switch
    {
        DefenceSessionType.Bachelor => "бакалаврську роботу",
        DefenceSessionType.Master => "магістерську роботу",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string SingularInstrumental(DefenceSessionType type) => type switch
    {
        DefenceSessionType.Bachelor => "бакалаврською роботою",
        DefenceSessionType.Master => "магістерською роботою",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string Plural(DefenceSessionType type) => type switch
    {
        DefenceSessionType.Bachelor => "бакалаврські роботи",
        DefenceSessionType.Master => "магістерські роботи",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string PluralCapitalized(DefenceSessionType type) => type switch
    {
        DefenceSessionType.Bachelor => "Бакалаврські роботи",
        DefenceSessionType.Master => "Магістерські роботи",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string GenitivePlural(DefenceSessionType type) => type switch
    {
        DefenceSessionType.Bachelor => "бакалаврських робіт",
        DefenceSessionType.Master => "магістерських робіт",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string MyWork(DefenceSessionType type) => $"Моя {Singular(type)}";

    public static string TopicSubmitted(DefenceSessionType type) =>
        $"Тему {GenitiveSingular(type)} подано на розгляд.";

    public static string NotCreatedYet(DefenceSessionType type) =>
        $"{SingularCapitalized(type)} ще не створено. Зверніться до адміністратора після додавання до сесії захисту.";

    public static string StudentAlreadyHasWork(DefenceSessionType type) =>
        $"Сесію не можна змінити — студент уже має {SingularAccusative(type)}.";

    public static string StudentLinkedToWork(DefenceSessionType type) =>
        $"Студента не можна видалити: він пов'язаний із {SingularInstrumental(type)}.";

    public const string GraduationWorksInstrumental = "випусковими роботами";

    private static string GenitiveSingular(DefenceSessionType type) => type switch
    {
        DefenceSessionType.Bachelor => "бакалаврської роботи",
        DefenceSessionType.Master => "магістерської роботи",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };
}
