using DiplomaManagementSystem.Domain.Enums;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaManagementSystem.Web;

public static class CheckpointOutcomeSelectList
{
    public static List<SelectListItem> Build()
    {
        return Enum.GetValues<CheckpointOutcome>()
            .Select(outcome => new SelectListItem(
                UkrainianDisplay.FormatCheckpointOutcome(outcome),
                outcome.ToString()))
            .ToList();
    }
}
