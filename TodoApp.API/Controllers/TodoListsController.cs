using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;
using TodoApp.Core.Services;

namespace TodoApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : CustomBaseController
    {
        private readonly ITodoListService _service;
        private readonly IMapper _mapper;

        public TodoListsController(UserManager<UserApp> userManager, ITodoListService service, IMapper mapper)
            : base(userManager)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] bool? isCompleted)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.GetAllIsCompleted(isCompleted, activeUser));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.GetByIdWithTodoItems(id, activeUser));
        }

        [HttpPost]
        public async Task<IActionResult> Save(TodoListCreateDto todoListCreateDto)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.CreateTodoList(todoListCreateDto, activeUser));
        }

        [HttpPut]
        public async Task<IActionResult> Update(TodoListUpdateDto todoListUpdateDto)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _service.UpdateTodoList(todoListUpdateDto, activeUser));
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
                await _service.RemoveTodoList(id, activeUser));
        }
    }
}