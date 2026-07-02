using DiplomaManagementSystem.Application.Documents;

namespace DiplomaManagementSystem.Application.Tests.Documents;

public sealed class PersonNameFileSegmentTests
{
    [Fact]
    public void FormatGenitive_UsesSurnameFirstOrder()
    {
        string segment = PersonNameFileSegment.FormatGenitive("Іваненко Іван");

        Assert.Equal("Ivanenka_Ivana", segment);
    }

    [Fact]
    public void FormatGenitive_WhenFullNameMissing_UsesPlaceholder()
    {
        string segment = PersonNameFileSegment.FormatGenitive(null);

        Assert.Equal("Nevidomyi", segment);
    }
}
