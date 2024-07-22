using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTOs;
using TodoApp.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace TodoApp.Web.Controllers;

[Authorize]
public class TodoItemsController : BaseController
{
    private readonly TodoItemApiService _todoItemApiService;
    private readonly TodoListApiService _todoListApiService;
    private readonly ILogger<TodoItemsController> _logger;

    public TodoItemsController(TodoItemApiService todoItemApiService, TodoListApiService todoListApiService, ILogger<TodoItemsController> logger)
    {
        _todoItemApiService = todoItemApiService;
        _todoListApiService = todoListApiService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int id)
    {
        var todoList = await _todoListApiService.GetByIdAsync(id, AccessToken);
        return View(todoList);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] TodoItemCreateDto newTodoItem)
    {
        if (ModelState.IsValid)
        {
            await _todoItemApiService.Save(newTodoItem, AccessToken);
            return Ok();
        }
        return BadRequest();
    }
    
    // [ServiceFilter(typeof(NotFoundFilter<TodoItem>))]
    public async Task<IActionResult> Update(int id)
    {
        var todoItem = await _todoItemApiService.GetByIdAsync(id, AccessToken);
        
        return View(todoItem);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TodoItemDto todoItemDto)
    {
        if (ModelState.IsValid)
        {
            await _todoItemApiService.Update(todoItemDto, AccessToken);
            return RedirectToAction(nameof(Index), new { id = todoItemDto.TodoListId });
        }
        return View();
    }

    [HttpPut]
    public async Task<IActionResult> IsCompletedTrigger([FromBody] IsCompletedTriggerDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _todoItemApiService.IsCompletedTrigger(dto, AccessToken);
            return Ok(result);
        }
        return BadRequest();
    }

    public async Task<IActionResult> Remove(int id, int todoListId)
    {
        await _todoItemApiService.Remove(id, AccessToken);
        return RedirectToAction(nameof(Index), new { id = todoListId });
    }
}