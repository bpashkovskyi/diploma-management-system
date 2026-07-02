using DiplomaManagementSystem.Application.Student.Dtos;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Student.Validation;

public sealed class SelectSupervisorValidator : AbstractValidator<SelectSupervisorDto>
{
    public SelectSupervisorValidator()
    {
        RuleFor(dto => dto.DiplomaId).NotEmpty();
        RuleFor(dto => dto.SupervisorId).NotEmpty();
    }
}
