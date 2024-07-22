using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.Models;

namespace TodoApp.Repository.Configurations;

internal class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.IsCompleted).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired();
        builder.Property(x => x.UpdatedDate);
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.HasOne(x => x.User)
            .WithMany(u => u.TodoLists)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("TodoLists");
        
        
    }
}