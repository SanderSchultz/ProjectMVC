using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProjectMVC.DTO;
using ProjectMVC.Services.Interfaces;

[Authorize]
public class PostController : Controller
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        var posts = await _postService.GetAllPostsAsync(userId, isAdmin);

        return View(posts);
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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _postService.CreatePostAsync(dto, userId);

		if(!result.Succeeded)
		{
			TempData["ErrorMessage"] = result.Error;
			return RedirectToAction(nameof(Index));
		}

		TempData["SuccessMessage"] = result.SuccessMessage;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "CanEditPost")]
    public async Task<IActionResult> Edit(int id, PostUpdateDto dto)
    {
		if ((dto.ImageFile == null || dto.ImageFile.Length == 0) && string.IsNullOrEmpty(dto.Title))
		{
			TempData["ErrorMessage"] = "You need to choose either a Picture or a Title to update post";
			return RedirectToAction(nameof(Index));
		}

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _postService.UpdatePostAsync(id, dto, userId);

		if(!result.Succeeded)
		{
			TempData["ErrorMessage"] = result.Error;
			return RedirectToAction(nameof(Index));
		}

		TempData["SuccessMessage"] = result.SuccessMessage;
        return RedirectToAction(nameof(Index));
    }

	[HttpPost]
	[ValidateAntiForgeryToken]
	[Authorize(Policy = "CanEditPost")]
	public async Task<IActionResult> Delete(int id)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var result = await _postService.DeletePostAsync(id, userId);

		if(!result.Succeeded)
		{
			TempData["ErrorMessage"] = result.Error;
			return RedirectToAction(nameof(Index));
		}

		TempData["SuccessMessage"] = result.SuccessMessage;
		return RedirectToAction(nameof(Index));
	}

}
