using DiplomaManagementSystem.Application.Admin.StudyGroups;
using DiplomaManagementSystem.Application.Admin.StudyGroups.Dtos;
using DiplomaManagementSystem.Application.Identity;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Exceptions;
using DiplomaManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DiplomaManagementSystem.Application.Tests.Admin;

public sealed class StudyGroupAdminServiceTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly StudyGroupAdminService _service;

    public StudyGroupAdminServiceTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _service = new StudyGroupAdminService(_dbContext);
    }

    [Fact]
    public async Task CreateAsync_WhenNameUniqueInSession_CreatesGroup()
    {
        Guid sessionId = await SeedSessionAsync();
        Guid id = await _service.CreateAsync(new StudyGroupFormDto(null, sessionId, "КН-41"));

        StudyGroup? group = await _dbContext.StudyGroups.FindAsync(id);
        Assert.NotNull(group);
        Assert.Equal("КН-41", group.Name);
        Assert.Equal(sessionId, group.DefenceSessionId);
    }

    [Fact]
    public async Task CreateAsync_WhenNameDuplicateInSameSession_Throws()
    {
        Guid sessionId = await SeedSessionAsync();
        await _service.CreateAsync(new StudyGroupFormDto(null, sessionId, "КН-41"));

        await Assert.ThrowsAsync<DomainException>(() =>
            _service.CreateAsync(new StudyGroupFormDto(null, sessionId, "КН-41")));
    }

    [Fact]
    public async Task CreateAsync_WhenSameNameInDifferentSessions_Allowed()
    {
        Guid sessionA = await SeedSessionAsync();
        Guid sessionB = await SeedSessionAsync(DefenceSessionType.Master);

        await _service.CreateAsync(new StudyGroupFormDto(null, sessionA, "КН-41"));
        Guid idB = await _service.CreateAsync(new StudyGroupFormDto(null, sessionB, "КН-41"));

        StudyGroup? groupB = await _dbContext.StudyGroups.FindAsync(idB);
        Assert.NotNull(groupB);
        Assert.Equal(sessionB, groupB.DefenceSessionId);
    }

    [Fact]
    public async Task DeleteAsync_WhenEmpty_RemovesGroup()
    {
        Guid sessionId = await SeedSessionAsync();
        Guid id = await _service.CreateAsync(new StudyGroupFormDto(null, sessionId, "КН-41"));

        await _service.DeleteAsync(id);

        Assert.Null(await _dbContext.StudyGroups.FindAsync(id));
    }

    [Fact]
    public async Task DeleteAsync_WhenHasStudents_Throws()
    {
        Guid sessionId = await SeedSessionAsync();
        Guid groupId = await _service.CreateAsync(new StudyGroupFormDto(null, sessionId, "КН-41"));
        _dbContext.Users.Add(new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            UserName = "student@test.com",
            FullName = "Student",
            UserKind = UserKind.Student,
            DefenceSessionId = sessionId,
            StudyGroupId = groupId,
            CreatedAt = DateTimeOffset.UtcNow,
        });
        await _dbContext.SaveChangesAsync();

        await Assert.ThrowsAsync<DomainException>(() => _service.DeleteAsync(groupId));
    }

    [Fact]
    public async Task GetAllAsync_WhenFilteredBySession_ReturnsOnlySessionGroups()
    {
        Guid sessionA = await SeedSessionAsync();
        Guid sessionB = await SeedSessionAsync(DefenceSessionType.Master);
        await _service.CreateAsync(new StudyGroupFormDto(null, sessionA, "КН-41"));
        await _service.CreateAsync(new StudyGroupFormDto(null, sessionB, "КН-42"));

        IReadOnlyList<StudyGroupListItemDto> items = await _service.GetAllAsync(sessionA);

        StudyGroupListItemDto item = Assert.Single(items);
        Assert.Equal("КН-41", item.Name);
    }

    private async Task<Guid> SeedSessionAsync(DefenceSessionType type = DefenceSessionType.Bachelor)
    {
        Guid sessionId = Guid.NewGuid();
        _dbContext.DefenceSessions.Add(new DefenceSession
        {
            Id = sessionId,
            Year = 2026,
            Type = type,
            Status = DefenceSessionStatus.Active,
            CreatedAt = DateTimeOffset.UtcNow,
        });
        await _dbContext.SaveChangesAsync();
        return sessionId;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
