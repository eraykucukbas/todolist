using TodoApp.Core.DTOs;
using TodoApp.Core.Models;

namespace TodoApp.Core.Services
{
    public interface ITodoItemService : IService<TodoItem>
    {
        Task<CustomResponseDto<NoContentDto>> IsCompletedTrigger(int id, UserApp activeUser);
        Task<CustomResponseDto<NoContentDto>> UpdateTodoItem(TodoItemUpdateDto todoItemUpdateDto, UserApp activeUser);
        Task<CustomResponseDto<TodoItemDto>> GetTodoItemByIdAsync(int id, UserApp activeUser);
        Task<CustomResponseDto<TodoItemDto>> AddTodoItemAsync(TodoItemCreateDto todoItemCreateDto, UserApp activeUser);
        Task<CustomResponseDto<NoContentDto>> RemoveTodoItemAsync(int id, UserApp activeUser);
    }
}