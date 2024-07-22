using TodoApp.Core.DTOs;
using TodoApp.Core.Models;

namespace TodoApp.Core.Services
{
    public interface ITodoListService : IService<TodoList>
    {
        Task<CustomResponseDto<TodoList>> CheckUserAccess(int todoListId, string userId);
        Task<CustomResponseDto<List<TodoListDto>>> GetAllIsCompleted(bool?  isCompleted,UserApp activeUser);
        Task<CustomResponseDto<TodoListWithTodoItemDto>> GetByIdWithTodoItems(int id,UserApp activeUser);
        Task<CustomResponseDto<NoContentDto>> IsCompletedTrigger(int id, UserApp activeUser);
        Task<CustomResponseDto<TodoListDto>> CreateTodoList(TodoListCreateDto todoListCreateDto, UserApp activeUser);
        Task<CustomResponseDto<NoContentDto>> UpdateTodoList(TodoListUpdateDto todoListUpdateDto, UserApp activeUser);
        Task<CustomResponseDto<NoContentDto>> RemoveTodoList(int id, UserApp activeUser);
    }
}
