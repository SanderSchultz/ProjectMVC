using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProjectMVC.Services.Interfaces;

[Authorize]
public class CommentController : Controller
{
	private readonly ICommentService _commentService;
	private readonly ILogger _logger;

	public CommentController(ICommentService commentService, ILogger<CommentController> logger)
	{
		_commentService = commentService;
		_logger = logger;
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

		try
		{
			_logger.LogInformation("CreateCommentAsync called at {Time}", DateTime.UtcNow);

			await _commentService.CreateCommentAsync(postId, content);

			return RedirectToAction("Index", "Post");

		} catch(Exception e)
		{
			_logger.LogError(e, "CreateCommentAsync failed at {Time}", DateTime.UtcNow);
			return RedirectToAction("Error", "Error");
		}

	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		
		try
		{
			_logger.LogInformation("DeleteCommentAsync called at {Time}", DateTime.UtcNow);
			var result = await _commentService.DeleteCommentAsync(id);

			if(!result.Succeeded)
			{
				TempData["ErrorMessage"] = result.Error;
				return RedirectToAction("Index", "Post");
			}

			return RedirectToAction("Index", "Post");

		} catch(Exception e)
		{
			_logger.LogError(e, "DeleteCommentAsync failed at {Time}", DateTime.UtcNow);
			return RedirectToAction("Error", "Error");
		}
	}
}
