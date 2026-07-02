namespace DiplomaManagementSystem.Web.Areas.Admin.Models;

public sealed class ImportEmployeesViewModel
{
    public IFormFile? File { get; set; }

    public ImportResultViewModel? Result { get; set; }
}
