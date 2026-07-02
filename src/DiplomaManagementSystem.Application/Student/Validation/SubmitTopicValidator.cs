using DiplomaManagementSystem.Application.Student.Dtos;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Student.Validation;

public sealed class SubmitTopicValidator : AbstractValidator<SubmitTopicDto>
{
    public SubmitTopicValidator()
    {
        RuleFor(dto => dto.DiplomaId).NotEmpty();
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .MaximumLength(500);
    }
}
