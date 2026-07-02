using DiplomaManagementSystem.Application.Documents;
using DiplomaManagementSystem.Application.Identity;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.Tests.Documents;

public sealed class DiplomaStoragePathBuilderTests
{
    // TC-APP-HLP-003
    [Fact]
    public void BuildFolderSegments_ReturnsExpectedSegments()
    {
        DefenceSession session = new()
        {
            Year = 2026,
            Type = DefenceSessionType.Bachelor,
            Semester = 2,
        };

        StudyGroup group = new() { Name = "КН-41" };
        ApplicationUser student = new() { FullName = "Іван Іваненко" };

        IReadOnlyList<string> segments = DiplomaStoragePathBuilder.BuildFolderSegments(session, group, student);

        Assert.Equal(4, segments.Count);
        Assert.Equal("2026", segments[0]);
        Assert.Contains("Бакалавр", segments[1], StringComparison.Ordinal);
        Assert.Equal("КН-41", segments[2]);
        Assert.Equal("Іван Іваненко", segments[3]);
    }

    // TC-APP-HLP-004
    [Fact]
    public void SanitizeSegment_ReplacesInvalidCharacters()
    {
        string sanitized = DiplomaStoragePathBuilder.SanitizeSegment(@"group/name:test");

        Assert.Equal("group_name_test", sanitized);
    }

    // TC-APP-HLP-005
    [Fact]
    public void SanitizeSegment_Empty_ReturnsUnknown()
    {
        Assert.Equal("unknown", DiplomaStoragePathBuilder.SanitizeSegment("   "));
    }
}
