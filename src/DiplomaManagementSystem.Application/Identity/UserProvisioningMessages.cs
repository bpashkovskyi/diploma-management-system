namespace DiplomaManagementSystem.Application.Identity;

internal static class UserProvisioningMessages
{
    public static string EmailAlreadyInUse(string email) =>
        $"Електронна пошта вже використовується ({email}).";

    public static bool IsEmailAlreadyInUse(Domain.Exceptions.DomainException exception) =>
        exception.Message.Contains("вже використовується", StringComparison.Ordinal);
}
