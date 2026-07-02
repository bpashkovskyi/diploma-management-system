using DiplomaManagementSystem.Application.Admin.AnnualRoles.Dtos;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Admin.AnnualRoles;

public sealed class AssignAnnualRoleValidator : AbstractValidator<AssignAnnualRoleDto>
{
    public AssignAnnualRoleValidator()
    {
        RuleFor(x => x.DefenceSessionId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}
