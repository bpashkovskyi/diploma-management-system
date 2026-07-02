namespace DiplomaManagementSystem.Application.AdminPreview.Contracts;

using DiplomaManagementSystem.Domain.Enums;

public interface IAdminPreviewUserPickerService
{
    Task<IReadOnlyList<AdminPreviewUserOption>> GetUsersAsync(
        UserKind userKind,
        CancellationToken cancellationToken = default);
}

public sealed record AdminPreviewUserOption(
    Guid Id,
    string FullName,
    string Email,
    string? Subtitle);
