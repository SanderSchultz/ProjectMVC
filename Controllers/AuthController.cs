using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectMVC.Models;
using System.Security.Claims;

namespace ProjectMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
        }

		public IActionResult Login()
		{
			var model = new LoginDto
			{
				Email = "",
				Password = ""
			};

			return View(model);
		}

		public IActionResult Register()
		{
			return View(new UserDto());
		}

        [HttpPost]
        public async Task<IActionResult> Register(UserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email already in use");
				return View(dto);
            }

			if(dto.Password != dto.ConfirmPassword){
				ModelState.AddModelError(string.Empty, "Passwords do not match");
				return View(dto);
			}

            var user = new ApplicationUser
            {
                UserName = dto.Email,
				NormalizedUserName = dto.Email.ToUpper(),
                Email = dto.Email,
				NormalizedEmail = dto.Email.ToUpper(),
                Name = dto.Name
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
				var claim = new Claim("Name", user.Name);
				await _userManager.AddClaimAsync(user, claim);

                await _signInManager.SignInAsync(user, isPersistent: false);

				TempData["SuccessMessage"] = $"Welcome {user.Name}!";
				TempData["MessageType"] = "login";

				return RedirectToAction("Index", "Post");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

			return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
				return RedirectToAction("Index", "Post");
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null || user.UserName == null)
                {
                    ModelState.AddModelError(string.Empty, "Wrong email or password");
					return View(dto);
                }

				var userClaims = await _userManager.GetClaimsAsync(user);
				if (!userClaims.Any(c => c.Type == "Name"))
				{
					var claim = new Claim("Name", user.Name);
					await _userManager.AddClaimAsync(user, claim);
				}

                var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
					await _signInManager.SignInAsync(user, isPersistent: false);

					TempData["SuccessMessage"] = $"Welcome {user.Name}!";
					TempData["MessageType"] = "login";

					return RedirectToAction("Index", "Post");
                }

                ModelState.AddModelError(string.Empty, "Invalid email or password");
				return View();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later");
				return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

			TempData["SuccessMessage"] = "You have been successfully logged out";
			TempData["MessageType"] = "logout";

			return RedirectToAction("Index", "Post");
        }

    }
}
