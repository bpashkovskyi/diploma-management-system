using DiplomaManagementSystem.Application.Documents.Dtos;
using DiplomaManagementSystem.Web.Areas.Student.Models;

namespace DiplomaManagementSystem.Web.Mapping;

internal static class DiplomaDocumentMapper
{
    public static DiplomaDocumentsViewModel Map(DiplomaDocumentsBundleDto? bundle)
    {
        if (bundle is null)
        {
            return new DiplomaDocumentsViewModel();
        }

        return new DiplomaDocumentsViewModel
        {
            StudentWorkVersions = bundle.StudentWorkVersions.Select(MapItem).ToList(),
            LatestSupervisorFeedback = bundle.LatestSupervisorFeedback is null
                ? null
                : MapItem(bundle.LatestSupervisorFeedback),
            LatestExternalReview = bundle.LatestExternalReview is null
                ? null
                : MapItem(bundle.LatestExternalReview),
            LatestAntiPlagiarismReport = bundle.LatestAntiPlagiarismReport is null
                ? null
                : MapItem(bundle.LatestAntiPlagiarismReport),
        };
    }

    private static DiplomaDocumentItemViewModel MapItem(DiplomaDocumentDto document) => new()
    {
        Id = document.Id,
        Kind = document.Kind,
        KindDisplay = UkrainianDisplay.FormatDiplomaDocumentKind(document.Kind),
        VersionNumber = document.VersionNumber,
        FileName = document.FileName,
        ViewUrl = document.ViewUrl,
        SizeBytes = document.SizeBytes,
        UploadedAt = document.UploadedAt,
    };
}
