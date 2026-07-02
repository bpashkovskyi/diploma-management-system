using DiplomaManagementSystem.Application.Employee.Dtos;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Employee.Validation;

public sealed class ReviewTopicRejectValidator : AbstractValidator<ReviewTopicDto>
{
    public ReviewTopicRejectValidator()
    {
        RuleFor(dto => dto.VersionId).NotEmpty();
        RuleFor(dto => dto.RejectionReason)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
