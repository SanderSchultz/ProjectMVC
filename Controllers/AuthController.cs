using Microsoft.AspNetCore.Mvc;
using ProjectMVC.DTO;
using ProjectMVC.Services.Interfaces;

namespace ProjectMVC.Controllers
{
    public class AuthController : Controller
    {
		private readonly IAuthService _authService;
		private readonly ILogger _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
			_authService = authService;
			_logger = logger;
        }

		/* Returns the main View filled with empty userDto */
		public IActionResult Register() => View(new UserDto());

		/* Resisters a user */
        [HttpPost]
        public async Task<IActionResult> Register(UserDto dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid data used in form");
				return View(dto);
            }

			try
			{
				_logger.LogInformation("RegisterUserAsync called at {Time}", DateTime.UtcNow);
				var result = await _authService.RegisterUserAsync(dto);
				if(!result.Succeeded)
				{
					ModelState.AddModelError(string.Empty, result.Error);
					return View(dto);
				}

				TempData["SuccessMessage"] = result.SuccessMessage;
				TempData["MessageType"] = "login";
				return RedirectToAction("Index", "Post");

			} catch(Exception e)
			{
				_logger.LogError(e, "RegisterUserAsync failed at {Time}", DateTime.UtcNow);
				return RedirectToAction("Error", "Error");
			}
        }

		/* Returns the main View filled with empty LoginDto */
		public IActionResult Login() => View(new LoginDto
											{
											Email = "",
											Password = ""
											});

		/* Logs in user */
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
			if(!ModelState.IsValid)
			{
                ModelState.AddModelError(string.Empty, "Invalid data used in form");
				return View(dto);
			}

			try
			{
				_logger.LogInformation("LoginUserAsync called at {Time}", DateTime.UtcNow);
				var result = await _authService.LoginUserAsync(dto);
				if(!result.Succeeded)
				{
					ModelState.AddModelError(string.Empty, result.Error);
					return View(dto);
				}

				/* If we log in, we return a message from the Service */
				TempData["SuccessMessage"] = result.SuccessMessage;
				TempData["MessageType"] = "login";
				return RedirectToAction("Index", "Post");

			} catch(Exception e)
			{
				_logger.LogError(e, "LoginUserAsync failed at {Time}", DateTime.UtcNow);
				return RedirectToAction("Error", "Error");
			}
        }

		/* Logs out user */
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
			try{

				_logger.LogError("LogoutUserAsync called at {Time}", DateTime.UtcNow);
				var result = await _authService.LogoutUserAsync();

				TempData["SuccessMessage"] = result.SuccessMessage;
				TempData["MessageType"] = "logout";

				return RedirectToAction("Index", "Post");

			} catch(Exception e)
			{
				_logger.LogError(e, "LogoutUserAsync failed at {Time}", DateTime.UtcNow);
				return RedirectToAction("Error", "Error");
			}
        }

    }
}
