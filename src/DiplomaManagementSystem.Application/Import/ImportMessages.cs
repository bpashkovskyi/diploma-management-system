namespace DiplomaManagementSystem.Application.Import;

internal static class ImportMessages
{
    public const string DefenceSessionNotFound = "Сесію захисту не знайдено.";

    public const string DefenceSessionArchived = "Сесія захисту заархівована.";

    public const string UnsupportedFileFormat = "Непідтримуваний формат файлу. Використовуйте .csv або .xlsx.";

    public static string RowValidationFailed(int rowNumber, string details) =>
        $"Рядок {rowNumber}: {details}";

    public static string EmailDomainNotAllowed(int rowNumber, string email) =>
        $"Рядок {rowNumber}: домен електронної пошти не дозволено ({email}).";

    public static string DuplicateEmail(int rowNumber, string email) =>
        $"Рядок {rowNumber}: дубльована електронна пошта ({email}).";

    public static string RowFailed(int rowNumber, string details) =>
        $"Рядок {rowNumber}: {details}";

    public static string InsufficientColumns(int rowNumber, int expected, int actual) =>
        $"Рядок {rowNumber}: недостатньо стовпців під час розбору файлу (очікується {expected}, отримано {actual}).";

    public static string MissingRequiredFields(int rowNumber) =>
        $"Рядок {rowNumber}: пропущено під час розбору файлу — відсутні обов'язкові поля (ПІБ або email).";
}
