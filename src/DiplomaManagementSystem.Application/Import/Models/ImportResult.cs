namespace DiplomaManagementSystem.Application.Import.Models;

public sealed class ImportResult
{
    public int TotalRows { get; init; }

    public int ImportedCount { get; init; }

    public int SkippedCount { get; init; }

    public IReadOnlyList<string> Errors { get; init; } = [];
}
