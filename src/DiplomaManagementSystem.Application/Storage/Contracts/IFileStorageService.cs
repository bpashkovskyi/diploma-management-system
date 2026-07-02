namespace DiplomaManagementSystem.Application.Storage.Contracts;

public interface IFileStorageService
{
    Task<string> EnsureFolderAsync(string? parentFolderId, string folderName, CancellationToken cancellationToken = default);

    Task<StoredFileResult> UploadFileAsync(
        string parentFolderId,
        string fileName,
        UploadFileContent content,
        CancellationToken cancellationToken = default);

    Task<string> GetViewUrlAsync(string fileId, CancellationToken cancellationToken = default);
}
