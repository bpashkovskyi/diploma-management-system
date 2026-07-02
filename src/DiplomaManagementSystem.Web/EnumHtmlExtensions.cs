using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaManagementSystem.Web;

public static class EnumHtmlExtensions
{
    public static IEnumerable<SelectListItem> GetLocalizedEnumSelectList<TEnum>(
        this IHtmlHelper html,
        TEnum? selected = null)
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .Select(value => new SelectListItem(
                UkrainianDisplay.FormatEnum(value),
                value.ToString(),
                selected.HasValue && EqualityComparer<TEnum>.Default.Equals(value, selected.Value)));
    }
}
