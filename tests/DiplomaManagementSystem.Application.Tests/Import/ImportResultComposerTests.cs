using DiplomaManagementSystem.Application.Import;
using DiplomaManagementSystem.Application.Import.Models;

namespace DiplomaManagementSystem.Application.Tests.Import;

public sealed class ImportResultComposerTests
{
    // TC-APP-IMP-001
    [Fact]
    public void Combine_MergesParseErrorsAndCounts()
    {
        ImportParseResult<EmployeeImportRow> parseResult = new()
        {
            Rows = [new EmployeeImportRow("Петро", "petro@test.local")],
            ParseErrors = ["Row 2: invalid email"],
        };

        ImportResult processed = new()
        {
            TotalRows = 1,
            ImportedCount = 1,
            SkippedCount = 0,
            Errors = ["Row 3: duplicate"],
        };

        ImportResult combined = ImportResultComposer.Combine(parseResult, processed);

        Assert.Equal(2, combined.TotalRows);
        Assert.Equal(1, combined.ImportedCount);
        Assert.Equal(1, combined.SkippedCount);
        Assert.Equal(2, combined.Errors.Count);
        Assert.Contains("invalid email", combined.Errors[0], StringComparison.Ordinal);
        Assert.Contains("duplicate", combined.Errors[1], StringComparison.Ordinal);
    }

    // TC-APP-IMP-002
    [Fact]
    public void Combine_WhenNoParseErrors_ReturnsProcessedCounts()
    {
        ImportParseResult<EmployeeImportRow> parseResult = new()
        {
            Rows =
            [
                new EmployeeImportRow("Петро", "petro@test.local"),
                new EmployeeImportRow("Олена", "olena@test.local"),
            ],
            ParseErrors = [],
        };

        ImportResult processed = new()
        {
            TotalRows = 2,
            ImportedCount = 2,
            SkippedCount = 0,
            Errors = [],
        };

        ImportResult combined = ImportResultComposer.Combine(parseResult, processed);

        Assert.Equal(2, combined.TotalRows);
        Assert.Equal(2, combined.ImportedCount);
        Assert.Equal(0, combined.SkippedCount);
        Assert.Empty(combined.Errors);
    }

    // TC-APP-IMP-003
    [Fact]
    public void Combine_WhenOnlyParseErrors_SkipsAllRows()
    {
        ImportParseResult<EmployeeImportRow> parseResult = new()
        {
            Rows = [],
            ParseErrors = ["Row 1: missing name", "Row 2: missing email"],
        };

        ImportResult processed = new()
        {
            TotalRows = 0,
            ImportedCount = 0,
            SkippedCount = 0,
            Errors = [],
        };

        ImportResult combined = ImportResultComposer.Combine(parseResult, processed);

        Assert.Equal(2, combined.TotalRows);
        Assert.Equal(0, combined.ImportedCount);
        Assert.Equal(2, combined.SkippedCount);
        Assert.Equal(2, combined.Errors.Count);
    }
}
