namespace DiplomaManagementSystem.Application.Options;

public sealed class ImportOptions
{
    public const string SectionName = "Import";

    public long MaxFileSizeBytes { get; set; } = 5_242_880;

    public string[] AllowedExtensions { get; set; } = [".csv", ".xlsx"];
}
