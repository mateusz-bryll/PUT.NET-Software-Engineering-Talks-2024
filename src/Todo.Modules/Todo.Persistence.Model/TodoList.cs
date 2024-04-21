namespace Todo.Persistence.Model;

public sealed class TodoList
{
    public int Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<TodoItem> ListItems { get; init; } = new HashSet<TodoItem>();

    public int TotalItems => ListItems.Count();
    public int TotalDoneItems => ListItems.Count(x => x.IsDone);
    public bool IsDone => ListItems.All(x => x.IsDone);
}