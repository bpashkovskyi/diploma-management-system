using DiplomaManagementSystem.Web.Areas.Secretary.Models;

namespace DiplomaManagementSystem.Web.Models.Shared;

public sealed class DiplomaListTableViewModel
{
    public IReadOnlyList<DiplomaListItemViewModel> Items { get; set; } = [];

    public bool ShowSupervisorColumn { get; set; } = true;

    public bool ShowDetailsLink { get; set; } = true;

    public string? DetailsArea { get; set; }

    public string? DetailsController { get; set; }

    public string DetailsAction { get; set; } = "Details";
}
