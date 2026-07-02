using DiplomaManagementSystem.Application.Admin.AnnualRoles.Contracts;
using DiplomaManagementSystem.Application.Admin.AnnualRoles.Dtos;
using DiplomaManagementSystem.Application.Persistence.Contracts;
using DiplomaManagementSystem.Application.ReadModels;
using DiplomaManagementSystem.Application.Secretary;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Application.Admin.AnnualRoles;

internal sealed class AnnualRoleService(IApplicationDbContext dbContext) : IAnnualRoleService
{
    private static readonly AnnualRoleType[] AllRoleTypes =
    [
        AnnualRoleType.DepartmentHead,
        AnnualRoleType.ExamCommissionSecretary,
        AnnualRoleType.AntiPlagiarismOfficer,
        AnnualRoleType.FormattingReviewer,
    ];

    public async Task<AnnualRolesPageDto?> GetPageAsync(Guid defenceSessionId, CancellationToken cancellationToken = default)
    {
        DefenceSession? session = await dbContext.DefenceSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == defenceSessionId, cancellationToken);

        if (session is null)
        {
            return null;
        }

        List<AnnualRoleAssignment> assignments = await dbContext.AnnualRoleAssignments
            .AsNoTracking()
            .Where(assignment => assignment.DefenceSessionId == defenceSessionId)
            .ToListAsync(cancellationToken);

        Dictionary<AnnualRoleType, AnnualRoleAssignment> byRole = assignments.ToDictionary(a => a.RoleType);

        List<PersonOptionDto> employees = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.UserKind == UserKind.Employee)
            .OrderBy(user => user.FullName)
            .Select(user => new PersonOptionDto(user.Id, user.FullName, user.Email ?? string.Empty))
            .ToListAsync(cancellationToken);

        List<AnnualRoleSlotDto> slots = AllRoleTypes
            .Select(roleType =>
            {
                if (!byRole.TryGetValue(roleType, out AnnualRoleAssignment? assignment))
                {
                    return new AnnualRoleSlotDto(roleType, null, null);
                }

                string? name = employees.FirstOrDefault(e => e.Id == assignment.EmployeeId)?.FullName;
                return new AnnualRoleSlotDto(roleType, assignment.EmployeeId, name);
            })
            .ToList();

        string sessionLabel = SecretarySessionLabel.Format(session.Year, session.Type, session.Semester);

        return new AnnualRolesPageDto(session.Id, sessionLabel, slots, employees);
    }

    public async Task AssignAsync(AssignAnnualRoleDto request, CancellationToken cancellationToken = default)
    {
        bool sessionExists = await dbContext.DefenceSessions.AnyAsync(
            session => session.Id == request.DefenceSessionId,
            cancellationToken);

        if (!sessionExists)
        {
            throw new DomainException($"Defence session {request.DefenceSessionId} not found.");
        }

        bool employeeExists = await dbContext.Users.AnyAsync(
            user => user.Id == request.EmployeeId && user.UserKind == UserKind.Employee,
            cancellationToken);

        if (!employeeExists)
        {
            throw new DomainException($"Employee {request.EmployeeId} not found.");
        }

        AnnualRoleAssignment? existing = await dbContext.AnnualRoleAssignments
            .FirstOrDefaultAsync(
                assignment => assignment.DefenceSessionId == request.DefenceSessionId
                              && assignment.RoleType == request.RoleType,
                cancellationToken);

        if (existing is not null)
        {
            existing.EmployeeId = request.EmployeeId;
            existing.AssignedAt = DateTimeOffset.UtcNow;
        }
        else
        {
            dbContext.AnnualRoleAssignments.Add(new AnnualRoleAssignment
            {
                Id = Guid.NewGuid(),
                DefenceSessionId = request.DefenceSessionId,
                EmployeeId = request.EmployeeId,
                RoleType = request.RoleType,
                AssignedAt = DateTimeOffset.UtcNow,
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
