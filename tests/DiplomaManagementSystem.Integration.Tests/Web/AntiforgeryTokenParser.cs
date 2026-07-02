using System.Text.RegularExpressions;

namespace DiplomaManagementSystem.Integration.Tests.Web;

internal static partial class AntiforgeryTokenParser
{
    public static string Parse(string html)
    {
        Match match = RequestVerificationTokenRegex().Match(html);
        if (!match.Success)
        {
            throw new InvalidOperationException("Antiforgery token was not found in the response HTML.");
        }

        return match.Groups[1].Value;
    }

    [GeneratedRegex("name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"", RegexOptions.IgnoreCase)]
    private static partial Regex RequestVerificationTokenRegex();
}
