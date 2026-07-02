namespace DiplomaManagementSystem.Application.Options;

public sealed class SecurityOptions
{
    public const string SectionName = "Security";

    public string[] AllowedEmailDomains { get; set; } = [];
}
