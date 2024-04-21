namespace Todo.Persistence.Model;

public sealed class TodoItem
{
    public int Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsDone { get; set; }
    public int ListId { get; init; }
}