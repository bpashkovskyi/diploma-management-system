using System.Net;
using DiplomaManagementSystem.Integration.Tests.Support;

namespace DiplomaManagementSystem.Integration.Tests.Web;

[Collection(nameof(IntegrationCollection))]
public sealed class AdminPreviewEndpointTests(PostgreSqlFixture fixture)
{
    [SkippableFact]
    public async Task PostSetStudentMode_RedirectsToSelectUser()
    {
        IntegrationTestGuards.RequireDatabase(fixture);
        Guid adminId = await IntegrationAdminHelper.CreateAdminUserAsync(fixture.CreateProvider());

        await using DiplomaManagementSystemWebApplicationFactory factory = fixture.CreateWebFactory();
        HttpClient client = IntegrationTestWebClient.CreateClient(factory, adminId);

        HttpResponseMessage homeResponse = await client.GetAsync("/Admin/Home/Index");
        homeResponse.EnsureSuccessStatusCode();
        string html = await homeResponse.Content.ReadAsStringAsync();
        string token = AntiforgeryTokenParser.Parse(html);

        FormUrlEncodedContent form = new(new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = token,
            ["mode"] = "2",
            ["returnUrl"] = "/",
        });

        HttpResponseMessage postResponse = await client.PostAsync("/AdminPreview/Set", form);

        Assert.Equal(HttpStatusCode.Redirect, postResponse.StatusCode);
        string? location = postResponse.Headers.Location?.ToString();
        Assert.NotNull(location);
        Assert.Contains("/AdminPreview/SelectUser", location, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("mode=Student", location, StringComparison.OrdinalIgnoreCase);
    }

    [SkippableFact]
    public async Task PostSetUser_RedirectsToStudentDiploma()
    {
        IntegrationTestGuards.RequireDatabase(fixture);
        IntegrationScenario scenario = await new IntegrationScenarioBuilder(fixture.CreateProvider())
            .SeedFullScenarioAsync();
        Guid adminId = await IntegrationAdminHelper.CreateAdminUserAsync(fixture.CreateProvider());

        await using DiplomaManagementSystemWebApplicationFactory factory = fixture.CreateWebFactory();
        HttpClient client = IntegrationTestWebClient.CreateClient(factory, adminId);

        HttpResponseMessage adminHome = await client.GetAsync("/Admin/Home/Index");
        adminHome.EnsureSuccessStatusCode();
        string adminHtml = await adminHome.Content.ReadAsStringAsync();
        string setToken = AntiforgeryTokenParser.Parse(adminHtml);

        FormUrlEncodedContent setForm = new(new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = setToken,
            ["mode"] = "2",
            ["returnUrl"] = "/",
        });

        HttpResponseMessage setResponse = await client.PostAsync("/AdminPreview/Set", setForm);
        Assert.Equal(HttpStatusCode.Redirect, setResponse.StatusCode);
        string? selectUserPath = setResponse.Headers.Location?.ToString();
        Assert.NotNull(selectUserPath);

        HttpResponseMessage selectUserResponse = await client.GetAsync(selectUserPath);
        selectUserResponse.EnsureSuccessStatusCode();
        string selectUserHtml = await selectUserResponse.Content.ReadAsStringAsync();
        string setUserToken = AntiforgeryTokenParser.Parse(selectUserHtml);

        FormUrlEncodedContent setUserForm = new(new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = setUserToken,
            ["userId"] = scenario.StudentId.ToString(),
            ["returnUrl"] = "/",
        });

        HttpResponseMessage setUserResponse = await client.PostAsync("/AdminPreview/SetUser", setUserForm);

        Assert.Equal(HttpStatusCode.Redirect, setUserResponse.StatusCode);
        Assert.Contains(
            "/Student/Diploma",
            setUserResponse.Headers.Location?.ToString(),
            StringComparison.OrdinalIgnoreCase);
    }
}
