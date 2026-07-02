using DiplomaManagementSystem.Application.Common;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Application.Tests.Common;

public sealed class ArchiveGuardTests
{
    private readonly ArchiveGuard _guard = new();

    // TC-APP-HLP-001
    [Fact]
    public void EnsureWritable_Active_DoesNotThrow()
    {
        DefenceSession session = new() { Status = DefenceSessionStatus.Active };

        Exception? exception = Record.Exception(() => _guard.EnsureWritable(session));

        Assert.Null(exception);
    }

    // TC-APP-HLP-002
    [Fact]
    public void EnsureWritable_Archived_Throws()
    {
        DefenceSession session = new() { Status = DefenceSessionStatus.Archived };

        Assert.Throws<DomainException>(() => _guard.EnsureWritable(session));
    }
}
