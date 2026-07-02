using DiplomaManagementSystem.Infrastructure.Storage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaManagementSystem.Web.Controllers;

[Authorize]
public sealed class LocalFilesController : Controller
{
    [HttpGet("/local-files/{encodedPath}")]
    public IActionResult Download(string encodedPath)
    {
        string filePath = LocalFilePathCodec.DecodePath(encodedPath);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        string contentType = "application/octet-stream";
        return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
    }
}
