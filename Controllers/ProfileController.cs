using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectMVC.Services.Interfaces;

namespace ProjectMVC.Controllers
{
    public class ProfileController : Controller
    {
		private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
			_profileService = profileService;
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
			var result = await _profileService.GetProfile(userId, actual_userId);

			if(!result.Succeeded)
			{
				TempData["ErrorMessage"] = result.Error;
				return RedirectToAction("Index", "Post");
			}

			return RedirectToAction("Index", "Post");

        }
    }
}
