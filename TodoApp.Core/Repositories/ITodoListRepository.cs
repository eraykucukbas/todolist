using TodoApp.Core.DTOs;
using TodoApp.Core.Models;

namespace TodoApp.Core.Repositories;

public interface ITodoListRepository : IGenericRepository<TodoList>
{
    Task<TodoList> GetByIdWithTodoItems(int id);
}