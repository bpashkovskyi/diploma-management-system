namespace DiplomaManagementSystem.Domain.Entities;

public sealed class DiplomaComment
{
    public Guid Id { get; set; }

    public Guid DiplomaId { get; set; }

    public Diploma Diploma { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public string Body { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}
