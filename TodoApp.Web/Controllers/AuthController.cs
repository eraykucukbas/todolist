using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTOs;
using TodoApp.Web.Services;

namespace TodoApp.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly AuthApiService _authenticationService;

        public AuthController(AuthApiService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid)
                return View(userCreateDto);

            var result = await _authenticationService.Register(userCreateDto);
            if (result.StatusCode == 200)
                return RedirectToAction("Login");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View(userCreateDto);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var result = await _authenticationService.Login(loginDto);
            if (result != null)
            {
                // Giriş başarılı, ana sayfaya yönlendir
                Response.Cookies.Append("AccessToken", result.AccessToken);
                return RedirectToAction("Index", "Home");
            }

            // Yeniden login sayfasına dön
            return View(loginDto);
        }
        
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            return RedirectToAction("Login", "Auth");
        }
    }
}