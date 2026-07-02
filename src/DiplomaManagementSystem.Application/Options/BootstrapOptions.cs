namespace DiplomaManagementSystem.Application.Options;

public sealed class BootstrapOptions
{
    public const string SectionName = "Bootstrap";

    public string AdminEmail { get; set; } = string.Empty;
}
