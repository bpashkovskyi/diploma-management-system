using DiplomaManagementSystem.Domain.Enums;

namespace DiplomaManagementSystem.Application.AdminPreview.Contracts;

public interface IAdminPreviewUserLookup
{
    Task<AdminPreviewUserProfile?> FindAsync(Guid userId, CancellationToken cancellationToken = default);
}

public sealed record AdminPreviewUserProfile(
    Guid Id,
    string FullName,
    string? Email,
    UserKind UserKind);
