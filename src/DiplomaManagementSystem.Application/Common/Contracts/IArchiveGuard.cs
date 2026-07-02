using DiplomaManagementSystem.Domain.Entities;

namespace DiplomaManagementSystem.Application.Common.Contracts;

public interface IArchiveGuard
{
    void EnsureWritable(DefenceSession session);
}
