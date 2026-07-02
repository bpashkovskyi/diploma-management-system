using DiplomaManagementSystem.Application.Secretary;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Tests.Secretary;

public sealed class SecretarySessionLabelTests
{
    // TC-APP-HLP-006
    [Fact]
    public void Format_BachelorWithSemester_ReturnsLabel()
    {
        string label = SecretarySessionLabel.Format(2026, DefenceSessionType.Bachelor, semester: 2);

        Assert.Equal("2026 — Бакалавр (сем. 2)", label);
    }

    // TC-APP-HLP-007
    [Fact]
    public void Format_MasterNoSemester_ReturnsLabel()
    {
        string label = SecretarySessionLabel.Format(2026, DefenceSessionType.Master, semester: null);

        Assert.Equal("2026 — Магістр", label);
    }
}
