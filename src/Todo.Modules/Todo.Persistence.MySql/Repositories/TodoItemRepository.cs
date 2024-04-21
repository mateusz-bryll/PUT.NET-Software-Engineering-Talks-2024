using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure.Abstractions;
using Todo.Persistence.Abstractions;
using Todo.Persistence.Model;
using Todo.Persistence.MySql.Database;

namespace Todo.Persistence.MySql.Repositories;

internal sealed class TodoItemRepository(TodoDatabaseContext _context, ISystemTime _systemTime) : ITodoItemRepository
{
    public async Task<int> CreateTodoAsync(int listId, string title, string? description, CancellationToken ct)
    {
        var todo = new TodoItem
        {
            CreatedAt = _systemTime.Now,
            Title = title,
            Description = description,
            IsDone = false
        };

        var list = await _context.Lists.FirstOrDefaultAsync(x => x.Id == listId, ct);
        if (list is null)
            throw new InvalidOperationException();
        
        list.ListItems.Add(todo);
        await _context.SaveChangesAsync(ct);

        return todo.Id;
    }

    public async Task<IEnumerable<TodoItem>> GetTodosAsync(int listId, CancellationToken ct)
    {
        var list = await _context.Lists
            .AsNoTracking()
            .Include(x => x.ListItems)
            .FirstOrDefaultAsync(x => x.Id == listId, ct);
        
        if (list is null)
            throw new InvalidOperationException();

        return list.ListItems;
    }

    public async Task<TodoItem> UpdateTodoAsync(int listId, int todoId, string? title, string? description, CancellationToken ct)
    {
        var todo = await _context.Items.FirstOrDefaultAsync(x => x.Id == todoId && x.ListId == listId, ct);
        if (todo is null)
            throw new InvalidOperationException();

        if (!string.IsNullOrEmpty(title))
            todo.Title = title;
        todo.Description = description;

        await _context.SaveChangesAsync(ct);

        return todo;
    }

    public async Task<bool> ToggleTodoStateAsync(int listId, int todoId, CancellationToken ct)
    {
        var todo = await _context.Items.FirstOrDefaultAsync(x => x.Id == todoId && x.ListId == listId, ct);
        if (todo is null)
            throw new InvalidOperationException();

        todo.IsDone = !todo.IsDone;
        
        await _context.SaveChangesAsync(ct);

        return todo.IsDone;
    }

    public async Task RemoveTodoAsync(int listId, int todoId, CancellationToken ct)
    {
        var todo = await _context.Items.FirstOrDefaultAsync(x => x.Id == todoId && x.ListId == listId, ct);
        if (todo is null)
            throw new InvalidOperationException();

        _context.Items.Remove(todo);
        await _context.SaveChangesAsync(ct);
    }
}