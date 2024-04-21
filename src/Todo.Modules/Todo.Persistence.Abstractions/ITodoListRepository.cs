using Todo.Persistence.Model;

namespace Todo.Persistence.Abstractions;

public interface ITodoListRepository
{
    Task<int> CreateListAsync(string title, string? description, CancellationToken ct);
    Task<IEnumerable<TodoList>> GetListsAsync(CancellationToken ct);
    Task<TodoList> UpdateList(int id, string? title, string? description, CancellationToken ct);
    Task RemoveListAsync(int id, CancellationToken ct);
}