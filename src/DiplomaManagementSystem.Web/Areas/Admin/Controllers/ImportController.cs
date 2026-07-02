using DiplomaManagementSystem.Application.Admin.DefenceSessions.Contracts;
using DiplomaManagementSystem.Application.Import.Contracts;
using DiplomaManagementSystem.Application.Import.Models;
using DiplomaManagementSystem.Application.Options;
using DiplomaManagementSystem.Web.Areas.Admin.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DiplomaManagementSystem.Web.Areas.Admin.Controllers;

public sealed class ImportController(
    IStudentImportService studentImportService,
    IEmployeeImportService employeeImportService,
    IDefenceSessionService defenceSessionService,
    IOptions<ImportOptions> importOptions) : AdminControllerBase(defenceSessionService)
{
    [HttpGet]
    public async Task<IActionResult> Students(Guid defenceSessionId, CancellationToken cancellationToken)
    {
        string? sessionLabel = await GetSessionLabelAsync(defenceSessionId, cancellationToken);
        if (sessionLabel is null)
        {
            return NotFound();
        }

        return View(new ImportStudentsViewModel
        {
            DefenceSessionId = defenceSessionId,
            SessionLabel = sessionLabel,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Students(ImportStudentsViewModel model, CancellationToken cancellationToken)
    {
        string? sessionLabel = await GetSessionLabelAsync(model.DefenceSessionId, cancellationToken);
        if (sessionLabel is null)
        {
            return NotFound();
        }

        model.SessionLabel = sessionLabel;

        if (model.File is null || model.File.Length == 0)
        {
            ModelState.AddModelError(nameof(model.File), "Оберіть файл.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string? validationError = ValidateFile(model.File!);
        if (validationError is not null)
        {
            ModelState.AddModelError(nameof(model.File), validationError);
            return View(model);
        }

        await using Stream stream = model.File!.OpenReadStream();
        ImportResult result = await studentImportService.ImportAsync(
            model.DefenceSessionId,
            stream,
            model.File.FileName,
            cancellationToken);
        model.Result = MapResult(result);

        return View(model);
    }

    [HttpGet]
    public IActionResult Employees()
    {
        return View(new ImportEmployeesViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Employees(ImportEmployeesViewModel model, CancellationToken cancellationToken)
    {
        if (model.File is null || model.File.Length == 0)
        {
            ModelState.AddModelError(nameof(model.File), "Оберіть файл.");
            return View(model);
        }

        string? validationError = ValidateFile(model.File);
        if (validationError is not null)
        {
            ModelState.AddModelError(nameof(model.File), validationError);
            return View(model);
        }

        await using Stream stream = model.File.OpenReadStream();
        ImportResult result = await employeeImportService.ImportAsync(stream, model.File.FileName, cancellationToken);
        model.Result = MapResult(result);

        return View(model);
    }

    private string? ValidateFile(IFormFile file)
    {
        ImportOptions options = importOptions.Value;
        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!options.AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            return $"Дозволені формати: {string.Join(", ", options.AllowedExtensions)}.";
        }

        if (file.Length > options.MaxFileSizeBytes)
        {
            return $"Максимальний розмір файлу: {options.MaxFileSizeBytes / 1024 / 1024} MB.";
        }

        return null;
    }

    private static ImportResultViewModel MapResult(ImportResult result)
    {
        return new ImportResultViewModel
        {
            TotalRows = result.TotalRows,
            ImportedCount = result.ImportedCount,
            SkippedCount = result.SkippedCount,
            Errors = result.Errors,
        };
    }
}
