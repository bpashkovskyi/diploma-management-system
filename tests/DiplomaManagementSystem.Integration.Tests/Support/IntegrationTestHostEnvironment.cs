using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace DiplomaManagementSystem.Integration.Tests.Support;

internal sealed class IntegrationTestHostEnvironment : IHostEnvironment
{
    public IntegrationTestHostEnvironment()
    {
        ContentRootPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "dms-integration-tests"));
        Directory.CreateDirectory(ContentRootPath);
        ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
    }

    public string EnvironmentName { get; set; } = Environments.Development;

    public string ApplicationName { get; set; } = "DiplomaManagementSystem.Integration.Tests";

    public string ContentRootPath { get; set; }

    public IFileProvider ContentRootFileProvider { get; set; }
}
