using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Domain.Entities;

public sealed class DiplomaDocument
{
    public Guid Id { get; set; }

    public Guid DiplomaId { get; set; }

    public Diploma Diploma { get; set; } = null!;

    public DiplomaDocumentKind Kind { get; set; }

    public int VersionNumber { get; set; }

    public string StorageFileId { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    public Guid UploadedById { get; set; }

    public DateTimeOffset UploadedAt { get; set; }

    public Guid? AdmissionStepAttemptId { get; set; }

    public DiplomaAdmissionStepAttempt? AdmissionStepAttempt { get; set; }
}
