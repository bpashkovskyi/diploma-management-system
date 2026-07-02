using DiplomaManagementSystem.Application.Employee.Dtos;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Employee.Validation;

public sealed class SupervisorRejectValidator : AbstractValidator<SupervisorActionDto>
{
    public SupervisorRejectValidator()
    {
        RuleFor(dto => dto.DiplomaId).NotEmpty();
        RuleFor(dto => dto.Comment)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
