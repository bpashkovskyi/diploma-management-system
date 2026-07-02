using DiplomaManagementSystem.Application.Employee.Dtos;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Employee.Validation;

public sealed class ApproveTopicValidator : AbstractValidator<ApproveTopicDto>
{
    public ApproveTopicValidator()
    {
        RuleFor(dto => dto.VersionId).NotEmpty();
        RuleFor(dto => dto.Comment)
            .MaximumLength(1000)
            .When(dto => !string.IsNullOrWhiteSpace(dto.Comment));
    }
}
