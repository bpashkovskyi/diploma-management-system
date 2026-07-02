using DiplomaManagementSystem.Application.Options;
using DiplomaManagementSystem.Application.Security;

namespace DiplomaManagementSystem.Application.Tests.Security;

public sealed class EmailDomainValidatorTests
{
    [Theory]
    [InlineData("student@university.edu.ua", true)]
    [InlineData("teacher@student.university.edu.ua", true)]
    [InlineData("user@gmail.com", false)]
    [InlineData("", false)]
    public void IsAllowed_ValidatesDomain(string email, bool expected)
    {
        EmailDomainValidator validator = new(Microsoft.Extensions.Options.Options.Create(new SecurityOptions
        {
            AllowedEmailDomains = ["university.edu.ua", "student.university.edu.ua"],
        }));

        bool result = validator.IsAllowed(email);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("no-at-sign")]
    [InlineData("trailing@")]
    public void IsAllowed_WhenEmailMalformed_ReturnsFalse(string email)
    {
        EmailDomainValidator validator = new(Microsoft.Extensions.Options.Options.Create(new SecurityOptions
        {
            AllowedEmailDomains = ["university.edu.ua"],
        }));

        Assert.False(validator.IsAllowed(email));
    }

    [Fact]
    public void IsAllowed_WhenNoDomainsConfigured_AllowsAnyValidEmail()
    {
        EmailDomainValidator validator = new(Microsoft.Extensions.Options.Options.Create(new SecurityOptions
        {
            AllowedEmailDomains = [],
        }));

        Assert.True(validator.IsAllowed("anyone@gmail.com"));
    }
}
