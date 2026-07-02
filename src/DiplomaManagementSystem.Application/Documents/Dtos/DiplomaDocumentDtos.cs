using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Documents.Dtos;

public sealed record DiplomaDocumentDto(
    Guid Id,
    DiplomaDocumentKind Kind,
    int VersionNumber,
    string FileName,
    string ViewUrl,
    long SizeBytes,
    DateTimeOffset UploadedAt,
    Guid? AdmissionStepAttemptId);

public sealed record DiplomaDocumentsBundleDto(
    IReadOnlyList<DiplomaDocumentDto> StudentWorkVersions,
    DiplomaDocumentDto? LatestSupervisorFeedback,
    DiplomaDocumentDto? LatestExternalReview,
    DiplomaDocumentDto? LatestAntiPlagiarismReport);
