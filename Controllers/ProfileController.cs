using Microsoft.AspNetCore.Mvc;
using ProjectMVC.DTO;
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

		public async Task<IActionResult> Profile()
		{

			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.MessageType = TempData["MessageType"];

			ViewBag.ErrorMessage = TempData["ErrorMessage"];

			if(ViewBag.ErrorMessage != null)
			{
				ModelState.AddModelError(string.Empty, ViewBag.ErrorMessage.ToString());
			}

			try
			{
				_logger.LogInformation("Profile called at {Time}", DateTime.UtcNow);
				var result = await _profileService.GetUserAsync();

				return View(result);

			} catch(Exception e)
			{
				_logger.LogError(e, "Profile failed at {Time}", DateTime.UtcNow);
				return RedirectToAction("Error", "Error");
			}

		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserDto dto)
        {

			ModelState.Remove(nameof(UserDto.Password));
			ModelState.Remove(nameof(UserDto.ConfirmPassword));

			try
			{
				_logger.LogInformation("EditProfile called at {Time}", DateTime.UtcNow);
				var result = await _profileService.UpdateProfile(dto);

				if(!result.Succeeded)
				{
					TempData["ErrorMessage"] = result.Error;
					return RedirectToAction(nameof(Profile));
				}

				TempData["SuccessMessage"] = result.SuccessMessage;
				return RedirectToAction(nameof(Profile));

			} catch(Exception e)
			{
				_logger.LogError(e, "EditProfile failed at {Time}", DateTime.UtcNow);
				return RedirectToAction("Error", "Error");
			}


        }
    }
}
