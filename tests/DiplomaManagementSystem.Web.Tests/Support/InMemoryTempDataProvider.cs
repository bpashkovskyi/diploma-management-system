using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DiplomaManagementSystem.Web.Tests.Support;

internal sealed class InMemoryTempDataProvider : ITempDataProvider
{
    private Dictionary<string, object?> _data = [];

    public IDictionary<string, object?> LoadTempData(HttpContext context) => _data;

    public void SaveTempData(HttpContext context, IDictionary<string, object?> values) =>
        _data = new Dictionary<string, object?>(values);
}
