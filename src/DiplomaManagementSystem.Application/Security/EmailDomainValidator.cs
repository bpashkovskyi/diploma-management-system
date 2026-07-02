using DiplomaManagementSystem.Application.Options;

using Microsoft.Extensions.Options;

namespace DiplomaManagementSystem.Application.Security;

public sealed class EmailDomainValidator(IOptions<SecurityOptions> options)
{
    public bool IsAllowed(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        int atIndex = email.LastIndexOf('@');
        if (atIndex < 0 || atIndex == email.Length - 1)
        {
            return false;
        }

        string domain = email[(atIndex + 1)..].ToLowerInvariant();
        string[] allowed = options.Value.AllowedEmailDomains;

        if (allowed.Length == 0)
        {
            return true;
        }

        return allowed.Any(d => string.Equals(d, domain, StringComparison.OrdinalIgnoreCase));
    }
}
