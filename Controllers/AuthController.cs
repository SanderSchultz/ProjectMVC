using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectMVC.Models;
using System.Security.Claims;

namespace ProjectMVC.Controllers
{
    public class AuthController : Controller
    {
		private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
			_authService = authService;
        }

		public IActionResult Register() => View(new UserDto());

        [HttpPost]
        public async Task<IActionResult> Register(UserDto dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid data used in form");
				return View(dto);
            }

			var result = await _authService.RegisterUserAsync(dto);
			if(!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, result.Error);
				return View(dto);
			}

			TempData["SuccessMessage"] = result.SuccessMessage;
			TempData["MessageType"] = "login";
			return RedirectToAction("Index", "Post");
        }

		public IActionResult Login() => View(new LoginDto
											{
											Email = "",
											Password = ""
											});

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
			if(!ModelState.IsValid)
			{
                ModelState.AddModelError(string.Empty, "Invalid data used in form");
				return View(dto);
			}

			var result = await _authService.LoginUserAsync(dto);
			if(!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, result.Error);
				return View(dto);
			}

			TempData["SuccessMessage"] = result.SuccessMessage;
			TempData["MessageType"] = "login";
			return RedirectToAction("Index", "Post");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
			var result = await _authService.LogoutUserAsync();

			TempData["SuccessMessage"] = result.SuccessMessage;
			TempData["MessageType"] = "logout";

			return RedirectToAction("Index", "Post");
        }

    }
}
