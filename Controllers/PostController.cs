using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProjectMVC.DTO;
using ProjectMVC.Services.Interfaces;

[Authorize]
public class PostController : Controller
{
    private readonly IPostService _postService;
    private readonly ILogger<PostController> _logger;

    public PostController(IPostService postService, ILogger<PostController> logger)
    {
        _postService = postService;
		_logger = logger;
    }

	[AllowAnonymous]
    public async Task<IActionResult> Index()
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
			_logger.LogInformation("GetAllPostsAsync called at {Time}", DateTime.UtcNow);
			var posts = await _postService.GetAllPostsAsync();
			return View(posts);

		} catch (Exception e)
		{

			_logger.LogError(e, "GetAllPostsAsync failed at {Time}", DateTime.UtcNow);
			return RedirectToAction("Error", "Error");

		}

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostCreateDto dto)
    {
        if (!ModelState.IsValid)
		{
			TempData["ErrorMessage"] = "Invalid data used in form";
			return RedirectToAction(nameof(Index));
		}

		try
		{
			_logger.LogInformation("CreatePostAsync called at {Time}", DateTime.UtcNow);
			var result = await _postService.CreatePostAsync(dto);

			if(!result.Succeeded)
			{
				TempData["ErrorMessage"] = result.Error;
				return RedirectToAction(nameof(Index));
			}

			TempData["SuccessMessage"] = result.SuccessMessage;
			return RedirectToAction(nameof(Index));

		} catch (Exception e)
		{

			_logger.LogError(e, "CreatePostAsync failed at {Time}", DateTime.UtcNow);
			return RedirectToAction("Error", "Error");

		}
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PostUpdateDto dto)
    {
		if ((dto.ImageFile == null || dto.ImageFile.Length == 0) && string.IsNullOrEmpty(dto.Title))
		{
			TempData["ErrorMessage"] = "You need to choose either a Picture or a Title to update post";
			return RedirectToAction(nameof(Index));
		}

		try
		{
			_logger.LogInformation("UpdatePostAsync called at {Time}", DateTime.UtcNow);
			var result = await _postService.UpdatePostAsync(id, dto);

			if(!result.Succeeded)
			{
				TempData["ErrorMessage"] = result.Error;
				return RedirectToAction(nameof(Index));
			}

			TempData["SuccessMessage"] = result.SuccessMessage;
			return RedirectToAction(nameof(Index));

		} catch (Exception e)
		{
			_logger.LogError(e, "UpdatePostAsync failed at {Time}", DateTime.UtcNow);
			return RedirectToAction("Error", "Error");
		}
    }

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			_logger.LogInformation("DeletePostAsync called at {Time}", DateTime.UtcNow);
			var result = await _postService.DeletePostAsync(id);

			if(!result.Succeeded)
			{
				TempData["ErrorMessage"] = result.Error;
				return RedirectToAction(nameof(Index));
			}

			TempData["SuccessMessage"] = result.SuccessMessage;
			return RedirectToAction(nameof(Index));

		} catch (Exception e)
		{
			_logger.LogError(e, "DeletePostAsync failed at {Time}", DateTime.UtcNow);
			return RedirectToAction("Error", "Error");
		}
	}

}
