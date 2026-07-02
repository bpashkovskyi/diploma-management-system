namespace DiplomaManagementSystem.Application;

public static class AuthorizationMessages
{
    public const string DiplomaNotFound = "Роботу не знайдено.";

    public const string TopicVersionNotFound = "Версію теми не знайдено.";

    public const string SessionArchived = "Сесія захисту заархівована — дії недоступні.";

    public const string NotSupervisor = "Ви не є керівником цієї роботи.";

    public const string NotReviewer = "Ви не є рецензентом цієї роботи.";

    public const string MissingSessionRole = "У вас немає потрібної ролі для цієї сесії захисту.";

    public const string NotDepartmentHead = "Ви не є завідувачем кафедри для цієї сесії захисту.";

    public const string UnsupportedAction = "Цю дію не підтримано.";

    public const string TopicVersionWrongState = "Тема не в очікуваному статусі розгляду.";

    public const string NotSecretaryForSession = "Ви не є секретарем ДЕК для цієї сесії захисту.";

    public const string SessionMismatch = "Робота не належить до обраної сесії захисту.";

    public const string ReviewerNotFound = "Рецензента не знайдено.";

    public const string SupervisorNotFound = "Обраного керівника не знайдено.";

    public const string AdmissionStepOverrideNotAllowed = "Цей крок допуску не можна перевизначити з таким результатом.";

    public const string AdmissionStepOverrideWrongStep = "Перевизначення секретаря дозволене лише для поточного кроку допуску.";
}
