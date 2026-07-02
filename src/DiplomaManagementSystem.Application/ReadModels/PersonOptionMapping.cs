using DiplomaManagementSystem.Application.Persistence;

namespace DiplomaManagementSystem.Application.ReadModels;

internal static class PersonOptionMapping
{
    public static PersonOptionDto From(UserOption option) =>
        new(option.Id, option.FullName, option.Email);

    public static List<PersonOptionDto> From(IEnumerable<UserOption> options) =>
        options.Select(From).ToList();
}
