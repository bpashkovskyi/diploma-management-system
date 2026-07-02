using DiplomaManagementSystem.Application.Admin.AnnualRoles;
using DiplomaManagementSystem.Application.Admin.AnnualRoles.Dtos;
using DiplomaManagementSystem.Application.Identity;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;
using DiplomaManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Application.Tests.Admin;

public sealed class AnnualRoleServiceTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AnnualRoleService _service;

    public AnnualRoleServiceTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _service = new AnnualRoleService(_dbContext);
    }

    // TC-APP-ADM-004a
    [Fact]
    public async Task GetPageAsync_WhenSessionMissing_ReturnsNull()
    {
        AnnualRolesPageDto? page = await _service.GetPageAsync(Guid.NewGuid());

        Assert.Null(page);
    }

    // TC-APP-ADM-004b
    [Fact]
    public async Task GetPageAsync_ReturnsAllRoleSlots()
    {
        Guid sessionId = await SeedSessionAsync();
        await SeedEmployeeAsync("Петро Петренко");

        AnnualRolesPageDto? page = await _service.GetPageAsync(sessionId);

        Assert.NotNull(page);
        Assert.Equal(sessionId, page.DefenceSessionId);
        Assert.Equal(4, page.Roles.Count);
        Assert.Contains(page.Roles, slot => slot.RoleType == AnnualRoleType.DepartmentHead);
        Assert.Contains(page.Roles, slot => slot.RoleType == AnnualRoleType.ExamCommissionSecretary);
    }

    // TC-APP-ADM-004c
    [Fact]
    public async Task AssignAsync_CreatesNewAssignment()
    {
        Guid sessionId = await SeedSessionAsync();
        Guid employeeId = await SeedEmployeeAsync("Олена Коваленко");

        await _service.AssignAsync(new AssignAnnualRoleDto(
            sessionId,
            AnnualRoleType.FormattingReviewer,
            employeeId));

        AnnualRoleAssignment? assignment = await _dbContext.AnnualRoleAssignments.SingleOrDefaultAsync(
            row => row.DefenceSessionId == sessionId && row.RoleType == AnnualRoleType.FormattingReviewer);

        Assert.NotNull(assignment);
        Assert.Equal(employeeId, assignment.EmployeeId);
    }

    // TC-APP-ADM-004d
    [Fact]
    public async Task AssignAsync_UpdatesExistingAssignment()
    {
        Guid sessionId = await SeedSessionAsync();
        Guid firstEmployeeId = await SeedEmployeeAsync("Петро Петренко", "petro@test.local");
        Guid secondEmployeeId = await SeedEmployeeAsync("Олена Коваленко", "olena@test.local");

        AssignAnnualRoleDto request = new(sessionId, AnnualRoleType.DepartmentHead, firstEmployeeId);
        await _service.AssignAsync(request);
        await _service.AssignAsync(request with { EmployeeId = secondEmployeeId });

        AnnualRoleAssignment assignment = await _dbContext.AnnualRoleAssignments.SingleAsync(
            row => row.DefenceSessionId == sessionId && row.RoleType == AnnualRoleType.DepartmentHead);

        Assert.Equal(secondEmployeeId, assignment.EmployeeId);
    }

    // TC-APP-ADM-004e
    [Fact]
    public async Task AssignAsync_WhenSessionMissing_Throws()
    {
        Guid employeeId = await SeedEmployeeAsync("Петро Петренко");

        await Assert.ThrowsAsync<DomainException>(() =>
            _service.AssignAsync(new AssignAnnualRoleDto(
                Guid.NewGuid(),
                AnnualRoleType.AntiPlagiarismOfficer,
                employeeId)));
    }

    // TC-APP-ADM-004f
    [Fact]
    public async Task AssignAsync_WhenEmployeeMissing_Throws()
    {
        Guid sessionId = await SeedSessionAsync();

        await Assert.ThrowsAsync<DomainException>(() =>
            _service.AssignAsync(new AssignAnnualRoleDto(
                sessionId,
                AnnualRoleType.AntiPlagiarismOfficer,
                Guid.NewGuid())));
    }

    // TC-APP-ADM-004g
    [Fact]
    public async Task GetPageAsync_ShowsAssignedEmployeeName()
    {
        Guid sessionId = await SeedSessionAsync();
        Guid employeeId = await SeedEmployeeAsync("Петро Петренко");
        await _service.AssignAsync(new AssignAnnualRoleDto(
            sessionId,
            AnnualRoleType.ExamCommissionSecretary,
            employeeId));

        AnnualRolesPageDto? page = await _service.GetPageAsync(sessionId);

        Assert.NotNull(page);
        AnnualRoleSlotDto slot = Assert.Single(page.Roles, role => role.RoleType == AnnualRoleType.ExamCommissionSecretary);
        Assert.Equal(employeeId, slot.AssignedEmployeeId);
        Assert.Equal("Петро Петренко", slot.AssignedEmployeeName);
    }

    private async Task<Guid> SeedSessionAsync()
    {
        Guid sessionId = Guid.NewGuid();
        _dbContext.DefenceSessions.Add(new DefenceSession
        {
            Id = sessionId,
            Year = 2026,
            Type = DefenceSessionType.Bachelor,
            Semester = 2,
            Status = DefenceSessionStatus.Active,
            CreatedAt = DateTimeOffset.UtcNow,
        });
        await _dbContext.SaveChangesAsync();
        return sessionId;
    }

    private async Task<Guid> SeedEmployeeAsync(string fullName, string? email = null)
    {
        Guid employeeId = Guid.NewGuid();
        string resolvedEmail = email ?? $"{employeeId:N}@test.local";
        _dbContext.Users.Add(new ApplicationUser
        {
            Id = employeeId,
            Email = resolvedEmail,
            UserName = resolvedEmail,
            FullName = fullName,
            UserKind = UserKind.Employee,
            CreatedAt = DateTimeOffset.UtcNow,
        });
        await _dbContext.SaveChangesAsync();
        return employeeId;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
