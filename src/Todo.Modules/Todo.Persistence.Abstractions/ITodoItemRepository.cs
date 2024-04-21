using Todo.Persistence.Model;

namespace Todo.Persistence.Abstractions;

public interface ITodoItemRepository
{
    Task<int> CreateTodoAsync(int listId, string title, string? description, CancellationToken ct);
    Task<IEnumerable<TodoItem>> GetTodosAsync(int listId, CancellationToken ct);
    Task<TodoItem> UpdateTodoAsync(int listId, int todoId, string? title, string? description, CancellationToken ct);
    Task<bool> ToggleTodoStateAsync(int listId, int todoId, CancellationToken ct);
    Task RemoveTodoAsync(int listId, int todoId, CancellationToken ct);
}