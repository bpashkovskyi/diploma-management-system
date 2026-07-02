using Microsoft.AspNetCore.Http;

namespace DiplomaManagementSystem.Web.Tests.Support;

internal sealed class FakeFormFile : IFormFile
{
    private readonly byte[] _content;

    public FakeFormFile(string fileName, byte[] content, string contentType = "application/pdf")
    {
        FileName = fileName;
        ContentType = contentType;
        _content = content;
    }

    public string ContentType { get; }

    public string ContentDisposition => string.Empty;

    public IHeaderDictionary Headers { get; } = new HeaderDictionary();

    public long Length => _content.Length;

    public string Name => "file";

    public string FileName { get; }

    public Stream OpenReadStream() => new MemoryStream(_content);

    public void CopyTo(Stream target) => OpenReadStream().CopyTo(target);

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) =>
        OpenReadStream().CopyToAsync(target, cancellationToken);
}
