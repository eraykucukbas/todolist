using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTOs;
using TodoApp.Web.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TodoApp.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly TodoListApiService _todoListApiService;

        public HomeController(TodoListApiService todoListApiService)
        {
            _todoListApiService = todoListApiService;
        }

        public async Task<IActionResult> Index(bool? isCompleted)
        {
            var todoLists = await _todoListApiService.GetAllAsync(isCompleted ?? false, AccessToken);
            ViewBag.IsCompleted = isCompleted ?? false;
            return View(todoLists);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] TodoListCreateDto newTodoList)
        {
            if (ModelState.IsValid)
            {
                await _todoListApiService.Save(newTodoList, AccessToken);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(TodoListDto todoListDto)
        {
            if (ModelState.IsValid)
            {
                await _todoListApiService.Update(todoListDto, AccessToken);
                return RedirectToAction(nameof(Index));
            }
            return View(todoListDto);
        }

        [HttpPut]
        public async Task<IActionResult> IsCompletedTrigger([FromBody] IsCompletedTriggerDto dto)
        {
            if (ModelState.IsValid)
            {
                await _todoListApiService.IsCompletedTrigger(dto, AccessToken);
                return Ok();
            }
            return BadRequest();
        }
        
        public async Task<IActionResult> Remove(int id)
        {
            await _todoListApiService.Remove(id, AccessToken);
            return RedirectToAction(nameof(Index));
        }

        // public async Task<IActionResult> Detail(int id)
        // {
        //     var todoList = await _todoListApiService.GetByIdAsync(id, AccessToken);
        //     return View(todoList);
        // }
    }
}