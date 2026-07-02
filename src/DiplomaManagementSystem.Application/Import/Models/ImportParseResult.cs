namespace DiplomaManagementSystem.Application.Import.Models;

public sealed class ImportParseResult<T>
{
    public IReadOnlyList<T> Rows { get; init; } = [];

    public IReadOnlyList<string> ParseErrors { get; init; } = [];
}
