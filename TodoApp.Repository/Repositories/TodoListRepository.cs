using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;
using TodoApp.Core.Repositories;
using TodoApp.Repository.Models;

namespace TodoApp.Repository.Repositories
{
    public class TodoListRepository : GenericRepository<TodoList>, ITodoListRepository
    {
        public TodoListRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<TodoList> GetByIdWithTodoItems(int id)
        {
            var todoList = await _context.TodoLists
                .Include(x => x.TodoItems)
                .FirstOrDefaultAsync(t => t.Id == id); 

            if (todoList == null)
            {
                return null; // or handle not found case as needed
            }

            todoList.TodoItems = todoList.TodoItems
                .OrderBy(i => i.IsCompleted)
                .ThenByDescending(t => t.CreatedDate)
                .ToList();

            return todoList;
        }
    }
}