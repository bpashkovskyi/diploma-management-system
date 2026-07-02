using DiplomaManagementSystem.Application.Documents.Dtos;
using DiplomaManagementSystem.Application.Storage;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Documents.Contracts;

public interface IDiplomaDocumentService
{
    Task<DiplomaDocumentsBundleDto> GetDocumentsAsync(Guid diplomaId, CancellationToken cancellationToken = default);

    Task<DiplomaDocumentDto> UploadStudentWorkAsync(
        Guid studentId,
        Guid diplomaId,
        UploadFileContent file,
        CancellationToken cancellationToken = default);

    Task<DiplomaDocumentDto> UploadCheckpointDocumentAsync(
        Guid uploadedById,
        Guid diplomaId,
        AdmissionStep step,
        Guid admissionStepAttemptId,
        UploadFileContent file,
        CancellationToken cancellationToken = default);

    Task<bool> HasStudentWorkAsync(Guid diplomaId, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<Guid, DiplomaDocumentDto>> GetLatestStudentWorkByDiplomaIdsAsync(
        IReadOnlyCollection<Guid> diplomaIds,
        CancellationToken cancellationToken = default);

    void ValidateUploadFile(UploadFileContent file);
}
