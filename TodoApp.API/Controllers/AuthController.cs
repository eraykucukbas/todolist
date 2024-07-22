using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;
using TodoApp.Core.Services;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService, UserManager<UserApp> userManager)
            : base(userManager)
        {
            _authenticationService = authenticationService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto userCreateDto)
        {
            var result = await _authenticationService.RegisterAsync(userCreateDto);
            return CreateActionResult(result);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authenticationService.LoginAsync(loginDto);
            return CreateActionResult(result);
        }
    }
}