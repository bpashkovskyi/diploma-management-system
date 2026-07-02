using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Documents;

public static class DiplomaDocumentNaming
{
    public static string GetWorkTypeToken(DefenceSessionType sessionType) => sessionType switch
    {
        DefenceSessionType.Bachelor => "Bakalavrska_Robota",
        DefenceSessionType.Master => "Magisterska_Robota",
        _ => throw new ArgumentOutOfRangeException(nameof(sessionType), sessionType, null),
    };

    public static string BuildFileName(
        DefenceSessionType sessionType,
        DiplomaDocumentKind kind,
        int version,
        string extension,
        string? studentFullName = null,
        string? actorFullName = null)
    {
        string normalizedExtension = extension.StartsWith('.') ? extension : $".{extension}";
        string versionSuffix = $"_v{version}{normalizedExtension.ToLowerInvariant()}";
        string studentSegment = PersonNameFileSegment.FormatGenitive(studentFullName);

        return kind switch
        {
            DiplomaDocumentKind.StudentWork =>
                $"{GetWorkTypeToken(sessionType)}_{studentSegment}{versionSuffix}",
            DiplomaDocumentKind.SupervisorFeedback =>
                $"Vidguk_Kerivnyka_{PersonNameFileSegment.FormatGenitive(actorFullName)}_na_robotu_{studentSegment}{versionSuffix}",
            DiplomaDocumentKind.ExternalReview =>
                $"Recenziya_{PersonNameFileSegment.FormatGenitive(actorFullName)}_na_robotu_{studentSegment}{versionSuffix}",
            DiplomaDocumentKind.AntiPlagiarismReport =>
                $"Antyplagiat_{PersonNameFileSegment.FormatGenitive(actorFullName)}_na_robotu_{studentSegment}{versionSuffix}",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };
    }

    public static bool RequiresFile(DiplomaDocumentKind kind) => kind switch
    {
        DiplomaDocumentKind.SupervisorFeedback => true,
        DiplomaDocumentKind.ExternalReview => true,
        DiplomaDocumentKind.AntiPlagiarismReport => true,
        DiplomaDocumentKind.StudentWork => true,
        _ => false,
    };

    public static bool RequiresFile(AdmissionStep step) => step switch
    {
        AdmissionStep.SupervisorFeedback => true,
        AdmissionStep.ExternalReview => true,
        AdmissionStep.AntiPlagiarismClearance => true,
        _ => false,
    };

    public static DiplomaDocumentKind ToDocumentKind(AdmissionStep step) => step switch
    {
        AdmissionStep.SupervisorFeedback => DiplomaDocumentKind.SupervisorFeedback,
        AdmissionStep.ExternalReview => DiplomaDocumentKind.ExternalReview,
        AdmissionStep.AntiPlagiarismClearance => DiplomaDocumentKind.AntiPlagiarismReport,
        _ => throw new ArgumentOutOfRangeException(nameof(step), step, "This admission step does not accept document uploads."),
    };
}
