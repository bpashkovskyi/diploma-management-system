using DiplomaManagementSystem.Application.Admin.DefenceSessions.Dtos;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Admin.DefenceSessions;

public sealed class DefenceSessionFormValidator : AbstractValidator<DefenceSessionFormDto>
{
    public DefenceSessionFormValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.Semester)
            .InclusiveBetween(1, 2)
            .When(x => x.Semester.HasValue);
    }
}
