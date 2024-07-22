namespace TodoApp.Core.DTOs
{
    public class TodoItemCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int TodoListId { get; set; }

    }
}
