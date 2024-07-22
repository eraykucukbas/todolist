using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.Models;

namespace TodoApp.Repository.Configurations;

internal class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
        builder.Property(x => x.IsCompleted).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired();
        builder.Property(x => x.UpdatedDate);
        builder.HasOne(x => x.TodoList)
            .WithMany(x => x.TodoItems)
            .HasForeignKey(x => x.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("TodoItems");
    }
}