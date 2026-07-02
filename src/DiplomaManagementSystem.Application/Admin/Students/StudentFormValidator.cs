using DiplomaManagementSystem.Application.Admin.Students.Dtos;
using DiplomaManagementSystem.Application.Security;

using FluentValidation;

namespace DiplomaManagementSystem.Application.Admin.Students;

public sealed class StudentFormValidator : AbstractValidator<StudentFormDto>
{
    public StudentFormValidator(EmailDomainValidator emailDomainValidator)
    {
        RuleFor(dto => dto.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256)
            .Must(emailDomainValidator.IsAllowed)
            .WithMessage("Email domain is not allowed.");

        RuleFor(dto => dto.DefenceSessionId)
            .NotEqual(Guid.Empty);

        RuleFor(dto => dto.StudyGroupId)
            .NotEqual(Guid.Empty);
    }
}
