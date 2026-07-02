using DiplomaManagementSystem.Application.Constants;
using DiplomaManagementSystem.Application.Identity.Contracts;
using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Application.Identity;

internal sealed class UserProvisioningService(
    IApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) : IUserProvisioningService
{
    public async Task<ApplicationUser> CreateStudentAsync(
        string fullName,
        string email,
        Guid defenceSessionId,
        Guid studyGroupId,
        CancellationToken cancellationToken = default)
    {
        await EnsureEmailAvailableCoreAsync(email, excludeUserId: null, cancellationToken);
        await EnsureStudentSessionAndGroupAsync(defenceSessionId, studyGroupId, cancellationToken);

        ApplicationUser user = BuildUser(fullName, email, UserKind.Student, studyGroupId, defenceSessionId);
        await CreateWithRoleAsync(user, RoleNames.Student, cancellationToken);
        return user;
    }

    public async Task<ApplicationUser> CreateEmployeeAsync(
        string fullName,
        string email,
        CancellationToken cancellationToken = default)
    {
        await EnsureEmailAvailableCoreAsync(email, excludeUserId: null, cancellationToken);

        ApplicationUser user = BuildUser(fullName, email, UserKind.Employee, studyGroupId: null, defenceSessionId: null);
        await CreateWithRoleAsync(user, RoleNames.Employee, cancellationToken);
        return user;
    }

    public async Task<StudyGroup> GetOrCreateStudyGroupByNameAsync(
        Guid defenceSessionId,
        string name,
        CancellationToken cancellationToken = default)
    {
        await EnsureSessionExistsAsync(defenceSessionId, cancellationToken);

        StudyGroup? group = dbContext.StudyGroups.Local
            .FirstOrDefault(
                studyGroup => studyGroup.DefenceSessionId == defenceSessionId && studyGroup.Name == name);

        if (group is null)
        {
            group = await dbContext.StudyGroups
                .FirstOrDefaultAsync(
                    studyGroup => studyGroup.DefenceSessionId == defenceSessionId && studyGroup.Name == name,
                    cancellationToken);
        }

        if (group is not null)
        {
            return group;
        }

        group = new StudyGroup
        {
            Id = Guid.NewGuid(),
            Name = name,
            DefenceSessionId = defenceSessionId,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        dbContext.StudyGroups.Add(group);
        return group;
    }

    public Task EnsureEmailAvailableAsync(
        string email,
        Guid? excludeUserId = null,
        CancellationToken cancellationToken = default) =>
        EnsureEmailAvailableCoreAsync(email, excludeUserId, cancellationToken);

    private async Task EnsureEmailAvailableCoreAsync(
        string email,
        Guid? excludeUserId,
        CancellationToken cancellationToken)
    {
        ApplicationUser? existing = await userManager.FindByEmailAsync(email);
        if (existing is not null && existing.Id != excludeUserId)
        {
            throw new DomainException(UserProvisioningMessages.EmailAlreadyInUse(email));
        }
    }

    private async Task EnsureStudentSessionAndGroupAsync(
        Guid defenceSessionId,
        Guid studyGroupId,
        CancellationToken cancellationToken)
    {
        DefenceSession? session = await dbContext.DefenceSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(defenceSession => defenceSession.Id == defenceSessionId, cancellationToken);

        if (session is null)
        {
            throw new DomainException("Defence session not found.");
        }

        if (session.Status == DefenceSessionStatus.Archived)
        {
            throw new DomainException("Defence session is archived.");
        }

        StudyGroup? group = dbContext.StudyGroups.Local
            .FirstOrDefault(studyGroup => studyGroup.Id == studyGroupId);

        if (group is null)
        {
            group = await dbContext.StudyGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(studyGroup => studyGroup.Id == studyGroupId, cancellationToken);
        }

        if (group is null)
        {
            throw new DomainException("Study group not found.");
        }

        if (group.DefenceSessionId != defenceSessionId)
        {
            throw new DomainException("Study group does not belong to the selected defence session.");
        }
    }

    private async Task EnsureSessionExistsAsync(Guid defenceSessionId, CancellationToken cancellationToken)
    {
        bool exists = await dbContext.DefenceSessions
            .AnyAsync(session => session.Id == defenceSessionId, cancellationToken);

        if (!exists)
        {
            throw new DomainException("Defence session not found.");
        }
    }

    private static ApplicationUser BuildUser(
        string fullName,
        string email,
        UserKind userKind,
        Guid? studyGroupId,
        Guid? defenceSessionId) =>
        new()
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            NormalizedUserName = email.ToUpperInvariant(),
            FullName = fullName,
            UserKind = userKind,
            StudyGroupId = studyGroupId,
            DefenceSessionId = defenceSessionId,
            CreatedAt = DateTimeOffset.UtcNow,
            EmailConfirmed = true,
        };

    private async Task CreateWithRoleAsync(
        ApplicationUser user,
        string roleName,
        CancellationToken cancellationToken)
    {
        IdentityResult createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            string details = string.Join("; ", createResult.Errors.Select(error => error.Description));
            throw new DomainException($"Failed to create user: {details}");
        }

        IdentityResult roleResult = await userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
        {
            string details = string.Join("; ", roleResult.Errors.Select(error => error.Description));
            throw new DomainException($"Failed to assign role: {details}");
        }
    }
}
