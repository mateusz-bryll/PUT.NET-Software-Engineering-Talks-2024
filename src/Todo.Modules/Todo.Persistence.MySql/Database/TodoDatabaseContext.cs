using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Todo.Persistence.Model;
using Todo.Persistence.MySql.EntityTypeConfigurations;

namespace Todo.Persistence.MySql.Database;

internal sealed class TodoDatabaseContext(DbContextOptions<TodoDatabaseContext> options) : DbContext(options)
{
    public DbSet<TodoList> Lists { get; set; }
    public DbSet<TodoItem> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TodoListEntityTypeConfiguration());
    }
}

internal sealed class TodoDatabaseContextDesignTimeContextFactory : IDesignTimeDbContextFactory<TodoDatabaseContext>
{
    public TodoDatabaseContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TodoDatabaseContext>()
            .UseMySql(new MySqlServerVersion("8.3.0"))
            .Options;

        return new TodoDatabaseContext(options);
    }
}