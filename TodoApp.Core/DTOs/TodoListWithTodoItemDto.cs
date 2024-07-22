using TodoApp.Core.Models;

namespace TodoApp.Core.DTOs
{
    public class TodoListWithTodoItemDto : TodoListDto
    {
        // public TodoItemDto TodoItem { get; set; }
        public List<TodoItemDto> TodoItems { get; set; } = new List<TodoItemDto>();
    }
}
