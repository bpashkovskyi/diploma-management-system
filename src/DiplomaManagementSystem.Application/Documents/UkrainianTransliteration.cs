using System.Text;

namespace DiplomaManagementSystem.Application.Documents;

internal static class UkrainianTransliteration
{
    private static readonly (string Source, string Latin)[] Replacements =
    [
        ("щ", "shch"),
        ("ж", "zh"),
        ("х", "kh"),
        ("ц", "ts"),
        ("ч", "ch"),
        ("ш", "sh"),
        ("є", "ie"),
        ("ю", "iu"),
        ("я", "ia"),
        ("ї", "i"),
        ("й", "i"),
        ("и", "y"),
        ("і", "i"),
        ("ґ", "g"),
        ("а", "a"),
        ("б", "b"),
        ("в", "v"),
        ("г", "h"),
        ("д", "d"),
        ("е", "e"),
        ("з", "z"),
        ("к", "k"),
        ("л", "l"),
        ("м", "m"),
        ("н", "n"),
        ("о", "o"),
        ("п", "p"),
        ("р", "r"),
        ("с", "s"),
        ("т", "t"),
        ("у", "u"),
        ("ф", "f"),
        ("ь", string.Empty),
        ("'", string.Empty),
        ("’", string.Empty),
    ];

    public static string ToLatin(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        string lower = value.Trim().ToLowerInvariant();
        StringBuilder builder = new(lower.Length * 2);
        int index = 0;

        while (index < lower.Length)
        {
            bool matched = false;
            foreach ((string source, string latin) in Replacements)
            {
                if (!lower.AsSpan(index).StartsWith(source, StringComparison.Ordinal))
                {
                    continue;
                }

                builder.Append(latin);
                index += source.Length;
                matched = true;
                break;
            }

            if (!matched)
            {
                char character = lower[index];
                builder.Append(char.IsAsciiLetterOrDigit(character) ? character : '_');
                index++;
            }
        }

        return builder.ToString();
    }

    public static string ToFileSegment(string? value)
    {
        string latin = ToLatin(value);
        if (latin.Length == 0)
        {
            return "Nevidomyi";
        }

        return char.ToUpperInvariant(latin[0]) + latin[1..];
    }
}
