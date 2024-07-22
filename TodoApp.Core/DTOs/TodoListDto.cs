namespace TodoApp.Core.DTOs
{
    public class TodoListDto : BaseDto
    {
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}