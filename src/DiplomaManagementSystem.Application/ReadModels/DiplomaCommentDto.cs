namespace DiplomaManagementSystem.Application.ReadModels;

public sealed record DiplomaCommentDto(string AuthorName, string Body, DateTimeOffset CreatedAt);
