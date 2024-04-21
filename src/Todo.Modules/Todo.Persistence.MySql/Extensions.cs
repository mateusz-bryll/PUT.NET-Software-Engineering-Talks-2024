using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Todo.Persistence.Abstractions;
using Todo.Persistence.MySql.Database;
using Todo.Persistence.MySql.Repositories;

namespace Todo.Persistence.MySql;

public static class Extensions
{
    public static IServiceCollection AddTodoMySqlPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TodoDatabaseContext>(builder =>
            builder.UseMySql(connectionString, new MySqlServerVersion("8.3.0")));

        services.AddTransient<ITodoListRepository, TodoListRepository>();
        services.AddTransient<ITodoItemRepository, TodoItemRepository>();
        
        return services;
    }

    public static void UseTodoMySqlPersistence(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<TodoDatabaseContext>();
        db.Database.Migrate();
    }
}