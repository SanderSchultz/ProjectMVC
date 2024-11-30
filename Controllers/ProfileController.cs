using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectMVC.Services.Interfaces;

namespace ProjectMVC.Controllers
{
    public class ProfileController : Controller
    {
		private readonly IProfileService _profileService;
		private readonly ILogger _logger;

        public ProfileController(IProfileService profileService, ILogger<ProfileController> logger)
        {
			_profileService = profileService;
			_logger = logger;
        }

		public IActionResult Profile()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string userId)
        {

			return RedirectToAction("Index", "Post");

            var actual_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			try
			{
				_logger.LogInformation("GetProfile called at {Time}", DateTime.UtcNow);
				var result = await _profileService.GetProfile(userId, actual_userId);

				if(!result.Succeeded)
				{
					TempData["ErrorMessage"] = result.Error;
					return RedirectToAction("Index", "Post");
				}

			} catch(Exception e)
			{
				_logger.LogError(e, "GetProfile failed at {Time}", DateTime.UtcNow);
				return RedirectToAction("Error", "Error");
			}

			return RedirectToAction("Index", "Post");

        }
    }
}
