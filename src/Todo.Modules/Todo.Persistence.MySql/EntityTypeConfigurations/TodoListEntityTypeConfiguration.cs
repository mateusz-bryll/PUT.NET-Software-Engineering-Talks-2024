using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Persistence.Model;

namespace Todo.Persistence.MySql.EntityTypeConfigurations;

internal sealed class TodoListEntityTypeConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.ToTable("TodoLists");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseMySqlIdentityColumn();
        builder.Property(x => x.Title)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(3000);
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        builder.Ignore(x => x.TotalItems);
        builder.Ignore(x => x.TotalDoneItems);
        builder.Ignore(x => x.IsDone);
        builder.HasMany(x => x.ListItems)
            .WithOne()
            .HasForeignKey(x => x.ListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}