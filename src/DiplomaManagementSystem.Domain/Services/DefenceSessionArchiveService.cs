using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Domain.Services;

public sealed class DefenceSessionArchiveService
{
    public void Archive(DefenceSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (session.Status == DefenceSessionStatus.Archived)
        {
            throw new DomainException("Defence session is already archived.");
        }

        session.Status = DefenceSessionStatus.Archived;
        session.ArchivedAt = DateTimeOffset.UtcNow;
    }
}
