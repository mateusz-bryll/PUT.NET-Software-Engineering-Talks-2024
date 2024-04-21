using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure.Abstractions;
using Todo.Persistence.Abstractions;
using Todo.Persistence.Model;
using Todo.Persistence.MySql.Database;

namespace Todo.Persistence.MySql.Repositories;

internal sealed class TodoListRepository(TodoDatabaseContext _context, ISystemTime _systemTime) : ITodoListRepository
{
    public async Task<int> CreateListAsync(string title, string? description, CancellationToken ct)
    {
        var list = new TodoList
        {
            CreatedAt = _systemTime.Now,
            Title = title,
            Description = description,
            ListItems = new List<TodoItem>()
        };

        _context.Lists.Add(list);
        await _context.SaveChangesAsync(ct);

        return list.Id;
    }

    public async Task<IEnumerable<TodoList>> GetListsAsync(CancellationToken ct)
    {
        return await _context.Lists
            .Include(x => x.ListItems)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<TodoList> UpdateList(int id, string? title, string? description, CancellationToken ct)
    {
        var list = await _context.Lists
            .Include(x => x.ListItems)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (list is null)
            throw new InvalidOperationException();

        if (!string.IsNullOrEmpty(title))
            list.Title = title;
        list.Description = description;

        await _context.SaveChangesAsync(ct);

        return list;
    }

    public async Task RemoveListAsync(int id, CancellationToken ct)
    {
        var list = await _context.Lists.FindAsync([ id ], cancellationToken: ct);

        if (list is null)
            throw new InvalidOperationException();

        _context.Lists.Remove(list);
        await _context.SaveChangesAsync(ct);
    }
}