using DiplomaManagementSystem.Application.Import.Models;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Import.Validation;

public sealed class StudentImportRowValidator : AbstractValidator<StudentImportRow>
{
    public StudentImportRowValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.GroupName)
            .NotEmpty()
            .MaximumLength(50);
    }
}
