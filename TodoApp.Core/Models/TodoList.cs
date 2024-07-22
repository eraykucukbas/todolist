namespace TodoApp.Core.Models;

public class TodoList : BaseEntity
{
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public ICollection<TodoItem> TodoItems { get; set; }
    public string UserId { get; set; }
    public UserApp User { get; set; }
}