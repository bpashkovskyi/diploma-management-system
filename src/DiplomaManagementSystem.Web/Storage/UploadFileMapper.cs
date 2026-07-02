using DiplomaManagementSystem.Application.Storage;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Web.Storage;

public static class UploadFileMapper
{
    public static UploadFileContent ToUploadContent(IFormFile file)
    {
        if (file.Length <= 0)
        {
            throw new DomainException("Файл не обрано.");
        }

        return new UploadFileContent(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            file.Length);
    }
}
