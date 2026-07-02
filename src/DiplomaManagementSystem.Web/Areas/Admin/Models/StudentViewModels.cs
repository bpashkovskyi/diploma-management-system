using DiplomaManagementSystem.Domain.Enums;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaManagementSystem.Web.Areas.Admin.Models;

public sealed class StudentListViewModel
{
    public Guid DefenceSessionId { get; set; }

    public string SessionLabel { get; set; } = string.Empty;

    public IReadOnlyList<StudentListItemViewModel> Items { get; set; } = [];
}

public sealed class StudentListItemViewModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string StudyGroupName { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class StudentFormViewModel
{
    public Guid? Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Guid DefenceSessionId { get; set; }

    public string SessionLabel { get; set; } = string.Empty;

    public Guid StudyGroupId { get; set; }

    public DefenceSessionType? SessionType { get; set; }

    public IReadOnlyList<SelectListItem> StudyGroups { get; set; } = [];
}

public sealed class StudentDetailsViewModel
{
    public Guid Id { get; set; }

    public Guid DefenceSessionId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string SessionLabel { get; set; } = string.Empty;

    public DefenceSessionType SessionType { get; set; }

    public string StudyGroupName { get; set; } = string.Empty;

    public bool HasDiploma { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public bool CanDelete => !HasDiploma;
}

public sealed class StudentDeleteViewModel
{
    public Guid Id { get; set; }

    public Guid DefenceSessionId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DefenceSessionType SessionType { get; set; }

    public bool HasDiploma { get; set; }

    public bool CanDelete => !HasDiploma;
}
