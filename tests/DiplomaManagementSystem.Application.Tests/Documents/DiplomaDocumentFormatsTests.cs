using DiplomaManagementSystem.Application.Documents;

namespace DiplomaManagementSystem.Application.Tests.Documents;

public sealed class DiplomaDocumentFormatsTests
{
    [Theory]
    [InlineData("work.pdf", "application/pdf", ".pdf", "application/pdf", true)]
    [InlineData("work.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", true)]
    [InlineData("work.odt", "application/vnd.oasis.opendocument.text", ".odt", "application/vnd.oasis.opendocument.text", true)]
    [InlineData("work.exe", "application/octet-stream", ".exe", "", false)]
    [InlineData("work", null, "", "", false)]
    public void TryResolve_ReturnsExpected(
        string fileName,
        string? contentType,
        string expectedExtension,
        string expectedMimeType,
        bool expectedSuccess)
    {
        bool success = DiplomaDocumentFormats.TryResolve(fileName, contentType, out string extension, out string mimeType);

        Assert.Equal(expectedSuccess, success);
        Assert.Equal(expectedExtension, extension);
        if (expectedSuccess)
        {
            Assert.Equal(expectedMimeType, mimeType);
        }
    }

    [Fact]
    public void TryResolve_WhenContentTypeIsOctetStream_AcceptsKnownExtension()
    {
        bool success = DiplomaDocumentFormats.TryResolve(
            "work.pdf",
            "application/octet-stream",
            out string extension,
            out string mimeType);

        Assert.True(success);
        Assert.Equal(".pdf", extension);
        Assert.Equal("application/pdf", mimeType);
    }

    [Fact]
    public void TryResolve_WhenContentTypeMismatches_ReturnsFalse()
    {
        bool success = DiplomaDocumentFormats.TryResolve(
            "work.pdf",
            "image/png",
            out _,
            out _);

        Assert.False(success);
    }

    [Fact]
    public void AllowedExtensions_ContainsSupportedFormats()
    {
        IReadOnlyCollection<string> extensions = DiplomaDocumentFormats.AllowedExtensions;

        Assert.Contains(".pdf", extensions);
        Assert.Contains(".docx", extensions);
        Assert.Contains(".odt", extensions);
    }
}
