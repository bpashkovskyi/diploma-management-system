using DiplomaManagementSystem.Application.Import.Models;
using DiplomaManagementSystem.Application.Import.Validation;

namespace DiplomaManagementSystem.Application.Tests.Import;

public sealed class StudentImportRowValidatorTests
{
    private readonly StudentImportRowValidator _validator = new();

    [Fact]
    public async Task Validate_ValidRow_Succeeds()
    {
        StudentImportRow row = new("Іваненко Іван", "ivan@university.edu.ua", "KN-41");

        FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(row);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_InvalidEmail_Fails()
    {
        StudentImportRow row = new("Іваненко Іван", "not-an-email", "KN-41");

        FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(row);

        Assert.False(result.IsValid);
    }
}
