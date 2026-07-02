namespace DiplomaManagementSystem.Web.Secretary;

public interface ISecretarySessionService
{
    Guid? GetSelectedSessionId(HttpContext httpContext);

    Task SetSelectedSessionAsync(
        HttpContext httpContext,
        Guid userId,
        Guid defenceSessionId,
        CancellationToken cancellationToken = default);
}
