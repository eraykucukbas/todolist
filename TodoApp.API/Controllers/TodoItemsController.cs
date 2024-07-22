using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Filters;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;
using TodoApp.Core.Services;

namespace TodoApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : CustomBaseController
    {
        private readonly ITodoItemService _service;
        private readonly IMapper _mapper;

        public TodoItemsController(UserManager<UserApp> userManager, ITodoItemService todoItemService, IMapper mapper)
            : base(userManager)
        {
            _service = todoItemService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.GetTodoItemByIdAsync(id, activeUser));
        }

        [HttpPost]
        public async Task<IActionResult> Save(TodoItemCreateDto todoItemCreateDto)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.AddTodoItemAsync(todoItemCreateDto, activeUser));
        }

        [HttpPut]
        public async Task<IActionResult> Update(TodoItemUpdateDto todoItemUpdateDto)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.UpdateTodoItem(todoItemUpdateDto, activeUser));
        }

        [HttpPut("isCompletedTrigger")]
        public async Task<IActionResult> IsCompletedTrigger([FromBody] IsCompletedTriggerDto dto)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.IsCompletedTrigger(dto.Id, activeUser));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.RemoveTodoItemAsync(id, activeUser));
        }
    }
}