namespace DiplomaManagementSystem.Application.Options;

public sealed class AppLocalizationOptions
{
    public const string SectionName = "Localization";

    public string DefaultCulture { get; set; } = "uk-UA";

    public string[] SupportedCultures { get; set; } = ["uk-UA"];
}
