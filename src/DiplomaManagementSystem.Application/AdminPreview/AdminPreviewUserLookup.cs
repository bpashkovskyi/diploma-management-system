using DiplomaManagementSystem.Application.AdminPreview.Contracts;
using DiplomaManagementSystem.Application.Identity;
using DiplomaManagementSystem.Application.Persistence.Contracts;

using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Application.AdminPreview;

internal sealed class AdminPreviewUserLookup(IApplicationDbContext dbContext) : IAdminPreviewUserLookup
{
    public async Task<AdminPreviewUserProfile?> FindAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        ApplicationUser? user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == userId, cancellationToken);

        return user is null
            ? null
            : new AdminPreviewUserProfile(user.Id, user.FullName, user.Email, user.UserKind);
    }
}
