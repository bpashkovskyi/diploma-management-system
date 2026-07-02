using System.Net;

namespace DiplomaManagementSystem.Integration.Tests.Support;

internal static class IntegrationTestHtmlAssertions
{
    public static void AssertContainsText(string html, string expected, bool ignoreCase = false)
    {
        StringComparison comparison = ignoreCase
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        if (html.Contains(expected, comparison))
        {
            return;
        }

        string decodedHtml = WebUtility.HtmlDecode(html);
        Assert.Contains(expected, decodedHtml, comparison);
    }
}
