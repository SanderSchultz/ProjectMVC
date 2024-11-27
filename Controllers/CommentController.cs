using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProjectMVC.Services;

[Authorize]
public class CommentController : Controller
{
	private readonly ICommentService _commentService;

	public CommentController(ICommentService commentService)
	{
		_commentService = commentService;
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(int postId, string content)
	{

		if(string.IsNullOrWhiteSpace(content))
		{
			ModelState.AddModelError(string.Empty, "Content cannot be empty");
			return RedirectToAction("Index", "Post");
		}

		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		await _commentService.CreateCommentAsync(postId, content, userId);

		return RedirectToAction("Index", "Post");

	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var result = await _commentService.DeleteCommentAsync(id, userId);

		if(!result.Succeeded)
		{
			TempData["ErrorMessage"] = result.Error;
			return RedirectToAction("Index", "Post");
		}

		TempData["SuccessMessage"] = result.SuccessMessage;
		return RedirectToAction("Index", "Post");
	}
}
