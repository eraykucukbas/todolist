namespace TodoApp.Core.Models;

public class TodoItem : BaseEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public int TodoListId { get; set; }
    public TodoList? TodoList { get; set; }
}