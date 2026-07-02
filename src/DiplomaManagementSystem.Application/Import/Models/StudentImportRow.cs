namespace DiplomaManagementSystem.Application.Import.Models;

public sealed record StudentImportRow(string FullName, string Email, string GroupName) : IImportRow;
