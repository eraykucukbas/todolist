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
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, UserManager<UserApp> userManager, ILogger<UserController> logger, IMapper mapper)
            : base(userManager)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyUser()
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _userService.GetMyUser(activeUser.UserName));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _userService.UpdateUserAsync(activeUser.UserName, userUpdateDto));
        }
        
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _userService.ChangePassword(activeUser.UserName, userChangePasswordDto));
        }

        [HttpDelete]
        public async Task<IActionResult> Remove()
        {
            return await ExecuteServiceAsync(async activeUser =>
                await _userService.RemoveAsync(activeUser.UserName));
        }
    }
}