namespace TodoApp.Core.DTOs
{
    public class TodoItemDto : BaseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int TodoListId { get; set; }
    }
}
