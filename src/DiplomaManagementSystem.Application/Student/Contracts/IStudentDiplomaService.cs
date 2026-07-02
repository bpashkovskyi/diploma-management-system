using DiplomaManagementSystem.Application.Storage;
using DiplomaManagementSystem.Application.Student.Dtos;

namespace DiplomaManagementSystem.Application.Student.Contracts;

public interface IStudentDiplomaService
{
    Task<MyDiplomaDto> GetMyDiplomaAsync(Guid studentId, CancellationToken cancellationToken = default);

    Task SelectSupervisorAsync(
        Guid studentId,
        SelectSupervisorDto request,
        CancellationToken cancellationToken = default);

    Task SubmitTopicAsync(
        Guid studentId,
        SubmitTopicDto request,
        CancellationToken cancellationToken = default);

    Task DeclareWorkReadyAsync(
        Guid studentId,
        Guid diplomaId,
        CancellationToken cancellationToken = default);

    Task UploadWorkAsync(
        Guid studentId,
        Guid diplomaId,
        UploadFileContent file,
        CancellationToken cancellationToken = default);
}
