using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectMVC.Services.Interfaces;

namespace ProjectMVC.Controllers
{
    public class LikeController : Controller
    {
		private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
			_likeService = likeService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int postId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _likeService.ToggleLikeAsync(postId, userId);

			if(!result.Succeeded)
			{
				TempData["ErrorMessage"] = result.Error;
				return RedirectToAction("Index", "Post");
			}

			return RedirectToAction("Index", "Post");

        }
    }
}
