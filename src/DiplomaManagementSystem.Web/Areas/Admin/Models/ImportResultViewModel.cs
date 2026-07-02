namespace DiplomaManagementSystem.Web.Areas.Admin.Models;

public sealed class ImportResultViewModel
{
    public int TotalRows { get; init; }

    public int ImportedCount { get; init; }

    public int SkippedCount { get; init; }

    public IReadOnlyList<string> Errors { get; init; } = [];
}
