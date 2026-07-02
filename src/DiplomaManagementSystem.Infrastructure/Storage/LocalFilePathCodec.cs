namespace DiplomaManagementSystem.Infrastructure.Storage;

public static class LocalFilePathCodec
{
    public static string EncodePath(string fullPath) =>
        Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(fullPath));

    public static string DecodePath(string encodedPath)
    {
        string decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedPath));
        return Path.GetFullPath(decoded);
    }
}
