using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectMVC.Services.Interfaces;

namespace ProjectMVC.Controllers
{
    public class LikeController : Controller
    {
		private readonly ILikeService _likeService;
		private readonly ILogger _logger;

        public LikeController(ILikeService likeService, ILogger<LikeController> logger)
        {
			_likeService = likeService;
			_logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int postId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			try
			{
				_logger.LogError("ToggleLikeAsync called at {Time}", DateTime.UtcNow);
				var result = await _likeService.ToggleLikeAsync(postId, userId!);

				if(!result.Succeeded)
				{
					TempData["ErrorMessage"] = result.Error;
					return RedirectToAction("Index", "Post");
				}

				return RedirectToAction("Index", "Post");

			} catch(Exception e)
			{
				_logger.LogError(e, "ToggleLikeAsync failed at {Time}", DateTime.UtcNow);
				return RedirectToAction("Error", "Error");
			}

        }
    }
}
