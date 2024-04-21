using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Persistence.Model;

namespace Todo.Persistence.MySql.EntityTypeConfigurations;

internal sealed class TodoItemEntityTypeConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("TodoItems");

        builder.HasKey(x => new { x.Id, x.ListId });
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseMySqlIdentityColumn();
        builder.Property(x => x.Title)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(3000);
        builder.Property(x => x.IsDone)
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}