namespace DiplomaManagementSystem.Application.Options;

public sealed class FileStorageOptions
{
    public const string SectionName = "FileStorage";

    public string Provider { get; set; } = "Local";

    public long MaxFileSizeBytes { get; set; } = 52_428_800;

    public string[] AllowedExtensions { get; set; } = [".pdf", ".docx", ".odt"];

    public LocalFileStorageOptions Local { get; set; } = new();

    public GoogleDriveOptions GoogleDrive { get; set; } = new();
}

public sealed class LocalFileStorageOptions
{
    public string RootPath { get; set; } = "App_Data/diploma-files";
}

public sealed class GoogleDriveOptions
{
    public string? ServiceAccountJsonPath { get; set; }

    public string? RootFolderId { get; set; }

    public string? ImpersonateUserEmail { get; set; }
}
