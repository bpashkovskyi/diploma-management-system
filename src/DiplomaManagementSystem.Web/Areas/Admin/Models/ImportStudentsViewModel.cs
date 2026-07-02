namespace DiplomaManagementSystem.Web.Areas.Admin.Models;

public sealed class ImportStudentsViewModel
{
    public Guid DefenceSessionId { get; set; }

    public string SessionLabel { get; set; } = string.Empty;

    public IFormFile? File { get; set; }

    public ImportResultViewModel? Result { get; set; }
}
