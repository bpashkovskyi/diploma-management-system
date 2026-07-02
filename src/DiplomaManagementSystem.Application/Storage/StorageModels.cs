namespace DiplomaManagementSystem.Application.Storage;

public sealed record StoredFileResult(string FileId, string FileName, string MimeType, long SizeBytes);

public sealed record UploadFileContent(
    Stream Content,
    string FileName,
    string ContentType,
    long Length);
