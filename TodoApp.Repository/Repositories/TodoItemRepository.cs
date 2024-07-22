using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.Models;
using TodoApp.Core.Repositories;
using TodoApp.Repository.Models;

namespace TodoApp.Repository.Repositories
{
    public class TodoItemRepository : GenericRepository<TodoItem>,  ITodoItemRepository
    {
        public TodoItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
