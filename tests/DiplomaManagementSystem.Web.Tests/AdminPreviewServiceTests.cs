using System.Security.Claims;

using DiplomaManagementSystem.Application.Constants;
using DiplomaManagementSystem.Web.AdminPreview;

using Microsoft.AspNetCore.Http;

namespace DiplomaManagementSystem.Web.Tests;

public sealed class AdminPreviewServiceTests
{
    private readonly AdminPreviewService _service = new();

    [Fact]
    public void SetMode_StoresSelectedModeForAdmin()
    {
        DefaultHttpContext httpContext = CreateAdminContext();

        _service.SetMode(httpContext, AdminPreviewMode.Student);

        Assert.Equal(AdminPreviewMode.Student, _service.GetMode(httpContext));
        Assert.True(_service.IsActivePreview(httpContext));
    }

    [Fact]
    public void SetMode_ThrowsForNonAdmin()
    {
        DefaultHttpContext httpContext = new();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, RoleNames.Student)],
            authenticationType: "test"));

        Assert.Throws<UnauthorizedAccessException>(() =>
            _service.SetMode(httpContext, AdminPreviewMode.Student));
    }

    [Fact]
    public void GetMode_ReturnsAdminWhenSessionMissing()
    {
        DefaultHttpContext httpContext = CreateAdminContext();

        Assert.Equal(AdminPreviewMode.Admin, _service.GetMode(httpContext));
        Assert.False(_service.IsActivePreview(httpContext));
    }

    [Fact]
    public void SetImpersonatedUserId_StoresUserForAdmin()
    {
        DefaultHttpContext httpContext = CreateAdminContext();
        Guid userId = Guid.NewGuid();

        _service.SetImpersonatedUserId(httpContext, userId);

        Assert.Equal(userId, _service.GetImpersonatedUserId(httpContext));
        Assert.True(_service.HasImpersonation(httpContext));
    }

    [Fact]
    public void SetMode_ToEmployee_ClearsImpersonationWhenSwitchingFromStudent()
    {
        DefaultHttpContext httpContext = CreateAdminContext();
        _service.SetMode(httpContext, AdminPreviewMode.Student);
        _service.SetImpersonatedUserId(httpContext, Guid.NewGuid());

        _service.SetMode(httpContext, AdminPreviewMode.Employee);

        Assert.Null(_service.GetImpersonatedUserId(httpContext));
    }

    [Fact]
    public void GetMode_MapsLegacySecretaryToEmployee()
    {
        DefaultHttpContext httpContext = CreateAdminContext();
        httpContext.Session.SetInt32("AdminPreviewMode", AdminPreviewModeRules.LegacyEmployeeStoredValue);

        Assert.Equal(AdminPreviewMode.Employee, _service.GetMode(httpContext));
    }

    [Fact]
    public void RequiresImpersonation_IsTrueForStudentEmployeeAndLegacyStoredValue()
    {
        Assert.True(_service.RequiresImpersonation(AdminPreviewMode.Student));
        Assert.True(_service.RequiresImpersonation(AdminPreviewMode.Employee));
        Assert.True(_service.RequiresImpersonation((AdminPreviewMode)AdminPreviewModeRules.LegacyEmployeeStoredValue));
    }

    [Fact]
    public void GetMode_UsesPrincipal_WhenHttpContextUserIsAnonymous()
    {
        DefaultHttpContext httpContext = CreateAdminContext();
        _service.SetMode(httpContext, AdminPreviewMode.Student);
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

        ClaimsPrincipal adminPrincipal = new(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, RoleNames.Admin)],
            authenticationType: "test"));

        Assert.Equal(AdminPreviewMode.Student, _service.GetMode(httpContext, adminPrincipal));
    }

    [Fact]
    public void GetImpersonatedUserId_UsesPrincipal_WhenHttpContextUserIsAnonymous()
    {
        DefaultHttpContext httpContext = CreateAdminContext();
        Guid userId = Guid.NewGuid();
        _service.SetImpersonatedUserId(httpContext, userId);
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

        ClaimsPrincipal adminPrincipal = new(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, RoleNames.Admin)],
            authenticationType: "test"));

        Assert.Equal(userId, _service.GetImpersonatedUserId(httpContext, adminPrincipal));
    }

    [Fact]
    public void IsAdmin_ReturnsTrue_WhenOriginalUserIdClaimPresent()
    {
        DefaultHttpContext httpContext = CreateAdminContext();
        ClaimsPrincipal impersonatedPrincipal = new(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, RoleNames.Student),
                new Claim(AdminPreviewClaimTypes.OriginalUserId, Guid.NewGuid().ToString()),
            ],
            authenticationType: "test"));

        Assert.True(_service.IsAdmin(impersonatedPrincipal, httpContext));
    }

    private static DefaultHttpContext CreateAdminContext()
    {
        DefaultHttpContext httpContext = new();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, RoleNames.Admin)],
            authenticationType: "test"));
        httpContext.Session = new TestSession();
        return httpContext;
    }

    private sealed class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new();

        public bool IsAvailable => true;

        public string Id { get; } = Guid.NewGuid().ToString();

        public IEnumerable<string> Keys => _store.Keys;

        public void Clear() => _store.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _store.Remove(key);

        public void Set(string key, byte[] value) => _store[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value!);
    }
}
