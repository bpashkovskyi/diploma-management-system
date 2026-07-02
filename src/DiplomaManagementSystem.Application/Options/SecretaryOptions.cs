namespace DiplomaManagementSystem.Application.Options;

public sealed class SecretaryOptions
{
    public const string SectionName = "Secretary";

    public string SelectedSessionCookieName { get; set; } = "SelectedDefenceSessionId";

    public int SessionCookieExpirationDays { get; set; } = 30;
}
