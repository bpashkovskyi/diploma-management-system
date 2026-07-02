using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Application.Secretary.Contracts;
using DiplomaManagementSystem.Application.Secretary.Dtos;
using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Secretary;

internal sealed class SecretaryDiplomaDetailsService(
    IDiplomaQueries diplomaQueries,
    DiplomaDetailsAssembler assembler) : ISecretaryDiplomaDetailsService
{
    public async Task<DiplomaDetailsDto?> GetDetailsAsync(
        Guid sessionId,
        Guid diplomaId,
        CancellationToken cancellationToken = default)
    {
        Diploma? diploma = await diplomaQueries.FindDetailsReadAsync(sessionId, diplomaId, cancellationToken);

        if (diploma is null)
        {
            return null;
        }

        DiplomaDetailsContext context = await assembler.LoadContextAsync(diploma, cancellationToken);
        DiplomaDetailsHistory history = DiplomaDetailsAssembler.BuildHistory(context);
        DiplomaDetailsScreenParts screenParts = DiplomaDetailsAssembler.BuildScreenParts(context);

        return DiplomaDetailsAssembler.Assemble(sessionId, context, history, screenParts);
    }
}
