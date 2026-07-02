using DiplomaManagementSystem.Application.Import.Models;

namespace DiplomaManagementSystem.Application.Import;

internal static class ImportResultComposer
{
    public static ImportResult Combine<T>(ImportParseResult<T> parseResult, ImportResult processed)
        where T : IImportRow
    {
        int parseSkipped = parseResult.ParseErrors.Count;

        return new ImportResult
        {
            TotalRows = parseResult.Rows.Count + parseSkipped,
            ImportedCount = processed.ImportedCount,
            SkippedCount = processed.SkippedCount + parseSkipped,
            Errors = [.. parseResult.ParseErrors, .. processed.Errors],
        };
    }
}
