using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        private readonly UserManager<UserApp> _userManager;

        public CustomBaseController(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            if (response.StatusCode == 204)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [NonAction]
        protected async Task<UserApp> GetActiveUserAsync()
        {
            var username = HttpContext.User.Identity?.Name;
            if (username == null)
            {
                return null;
            }

            return await _userManager.FindByNameAsync(username);
        }

        [NonAction]
        protected async Task<IActionResult> ExecuteServiceAsync<T>(Func<UserApp, Task<CustomResponseDto<T>>> serviceFunc)
        {
            var activeUser = await GetActiveUserAsync();
            if (activeUser == null)
            {
                return CreateActionResult(CustomResponseDto<T>.Fail(404, "User not found"));
            }

            return CreateActionResult(await serviceFunc(activeUser));
        }
    }
}