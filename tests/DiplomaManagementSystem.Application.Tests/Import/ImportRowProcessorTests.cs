using DiplomaManagementSystem.Application.Identity;
using DiplomaManagementSystem.Application.Import;
using DiplomaManagementSystem.Application.Import.Models;
using DiplomaManagementSystem.Application.Import.Validation;
using DiplomaManagementSystem.Application.Options;
using DiplomaManagementSystem.Application.Security;
using DiplomaManagementSystem.Domain.Exceptions;

namespace DiplomaManagementSystem.Application.Tests.Import;

public sealed class ImportRowProcessorTests
{
    [Fact]
    public async Task ProcessAsync_DuplicateEmailFromProvisioning_ReturnsUkrainianDuplicateMessage()
    {
        EmailDomainValidator emailDomainValidator = new(
            Microsoft.Extensions.Options.Options.Create(new SecurityOptions()));
        ImportRowProcessor processor = new(emailDomainValidator);
        EmployeeImportRowValidator validator = new();

        ImportResult result = await processor.ProcessAsync(
            [new EmployeeImportRow("Петренко Петро", "dup@test.local")],
            validator,
            (_, _) => throw new DomainException(UserProvisioningMessages.EmailAlreadyInUse("dup@test.local")));

        Assert.Equal(0, result.ImportedCount);
        Assert.Equal(1, result.SkippedCount);
        Assert.Single(result.Errors);
        Assert.Contains("дубльована електронна пошта", result.Errors[0], StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessAsync_ValidationFailure_SkipsRowWithDetails()
    {
        EmailDomainValidator emailDomainValidator = new(
            Microsoft.Extensions.Options.Options.Create(new SecurityOptions()));
        ImportRowProcessor processor = new(emailDomainValidator);
        EmployeeImportRowValidator validator = new();

        ImportResult result = await processor.ProcessAsync(
            [new EmployeeImportRow(string.Empty, "bad-email")],
            validator,
            (_, _) => Task.CompletedTask);

        Assert.Equal(0, result.ImportedCount);
        Assert.Equal(1, result.SkippedCount);
        Assert.Single(result.Errors);
        Assert.StartsWith("Рядок 1:", result.Errors[0], StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessAsync_DisallowedEmailDomain_SkipsRow()
    {
        EmailDomainValidator emailDomainValidator = new(
            Microsoft.Extensions.Options.Options.Create(new SecurityOptions
            {
                AllowedEmailDomains = ["university.edu.ua"],
            }));
        ImportRowProcessor processor = new(emailDomainValidator);
        EmployeeImportRowValidator validator = new();

        ImportResult result = await processor.ProcessAsync(
            [new EmployeeImportRow("Петренко Петро", "user@gmail.com")],
            validator,
            (_, _) => Task.CompletedTask);

        Assert.Equal(0, result.ImportedCount);
        Assert.Equal(1, result.SkippedCount);
        Assert.Contains("домен електронної пошти не дозволено", result.Errors[0], StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessAsync_UnexpectedException_SkipsRowWithMessage()
    {
        EmailDomainValidator emailDomainValidator = new(
            Microsoft.Extensions.Options.Options.Create(new SecurityOptions()));
        ImportRowProcessor processor = new(emailDomainValidator);
        EmployeeImportRowValidator validator = new();

        ImportResult result = await processor.ProcessAsync(
            [new EmployeeImportRow("Петренко Петро", "user@test.local")],
            validator,
            (_, _) => throw new InvalidOperationException("db down"));

        Assert.Equal(0, result.ImportedCount);
        Assert.Equal(1, result.SkippedCount);
        Assert.Contains("db down", result.Errors[0], StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessAsync_ValidRow_ImportsSuccessfully()
    {
        EmailDomainValidator emailDomainValidator = new(
            Microsoft.Extensions.Options.Options.Create(new SecurityOptions()));
        ImportRowProcessor processor = new(emailDomainValidator);
        EmployeeImportRowValidator validator = new();
        int importCalls = 0;

        ImportResult result = await processor.ProcessAsync(
            [new EmployeeImportRow("Петренко Петро", "user@test.local")],
            validator,
            (_, _) =>
            {
                importCalls++;
                return Task.CompletedTask;
            });

        Assert.Equal(1, result.ImportedCount);
        Assert.Equal(0, result.SkippedCount);
        Assert.Empty(result.Errors);
        Assert.Equal(1, importCalls);
    }
}
