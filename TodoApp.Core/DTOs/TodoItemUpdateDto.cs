namespace TodoApp.Core.DTOs
{
    public class TodoItemUpdateDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
