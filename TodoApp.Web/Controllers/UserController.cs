using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTOs;
using TodoApp.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace TodoApp.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly UserApiService _userApiService;

        public UserController(UserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userApiService.GetMyUserAsync(AccessToken);
            return View(user);
        } 

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var success = await _userApiService.UpdateUserAsync(userUpdateDto, AccessToken);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to update user");
            }
            return View(userUpdateDto);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            if (ModelState.IsValid)
            {
                var success = await _userApiService.ChangePasswordAsync(userChangePasswordDto, AccessToken);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to change password");
            }
            return View(userChangePasswordDto);
        }

        [HttpPost]
        public async Task<IActionResult> Remove()
        {
            var success = await _userApiService.RemoveUserAsync(AccessToken);
            if (success)
            {
                return RedirectToAction("Logout", "Auth"); // Assuming there's a Logout action in AuthController
            }
            ModelState.AddModelError("", "Failed to remove user");
            return RedirectToAction(nameof(Index));
        }
    }
}