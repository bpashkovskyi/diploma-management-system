using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Persistence.Contracts;

public interface IDiplomaCommentQueries
{
    Task<List<DiplomaComment>> ListForDiplomaReadAsync(
        Guid diplomaId,
        CancellationToken cancellationToken = default);
}
