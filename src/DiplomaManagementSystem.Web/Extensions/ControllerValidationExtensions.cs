using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;

namespace DiplomaManagementSystem.Web.Extensions;

internal static class ControllerValidationExtensions
{
    public static async Task<bool> TryValidateFormAsync<T>(
        this Controller controller,
        IValidator<T> validator,
        T dto,
        CancellationToken cancellationToken = default)
    {
        ValidationResult result = await validator.ValidateAsync(dto, cancellationToken);
        if (result.IsValid)
        {
            return true;
        }

        foreach (ValidationFailure failure in result.Errors)
        {
            controller.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
        }

        return false;
    }
}
