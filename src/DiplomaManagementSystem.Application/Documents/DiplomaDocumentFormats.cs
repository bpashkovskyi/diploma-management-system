namespace DiplomaManagementSystem.Application.Documents;

public static class DiplomaDocumentFormats
{
    private static readonly Dictionary<string, string> ExtensionToMimeType = new(StringComparer.OrdinalIgnoreCase)
    {
        [".pdf"] = "application/pdf",
        [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".odt"] = "application/vnd.oasis.opendocument.text",
    };

    public static bool TryResolve(string fileName, string? contentType, out string extension, out string mimeType)
    {
        extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            mimeType = string.Empty;
            return false;
        }

        if (!ExtensionToMimeType.TryGetValue(extension, out mimeType!))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(contentType)
            && !string.Equals(contentType, mimeType, StringComparison.OrdinalIgnoreCase)
            && !contentType.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
        {
            return ExtensionToMimeType.Values.Any(value =>
                string.Equals(value, contentType, StringComparison.OrdinalIgnoreCase));
        }

        return true;
    }

    public static IReadOnlyCollection<string> AllowedExtensions => ExtensionToMimeType.Keys;
}
