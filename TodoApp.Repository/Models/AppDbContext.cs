using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoApp.Core.Models;
using TodoApp.Core.Models;
using TodoApp.Core.Models;

namespace TodoApp.Repository.Models
{
    public class AppDbContext : IdentityDbContext<UserApp>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entityReference.CreatedDate = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                            entityReference.UpdatedDate = DateTime.Now;
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entityReference.CreatedDate = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                            entityReference.UpdatedDate = DateTime.Now;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}