using DiplomaManagementSystem.Application.Common.Contracts;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Application.Common;

internal sealed class ArchiveGuard : IArchiveGuard
{
    public void EnsureWritable(DefenceSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (session.Status == DefenceSessionStatus.Archived)
        {
            throw new DomainException("Defence session is archived.");
        }
    }
}
