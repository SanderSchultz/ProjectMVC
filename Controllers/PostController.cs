using Microsoft.AspNetCore.Mvc;
using ProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        var posts = await _postService.GetAllPostsAsync(userId, isAdmin);

        _logger.LogInformation($"Fetched {posts.Count} posts");

        return View(posts);
    }

    // public IActionResult Create()
    // {
    //     return View();
    // }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _postService.CreatePostAsync(dto, userId);

        return RedirectToAction(nameof(Index));
    }

    // [Authorize(Policy = "CanEditPost")]
    // public async Task<IActionResult> Edit(int id)
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     var post = await _postService.GetPostForEditAsync(id, userId);
    //
    //     if (post == null)
    //         return NotFound();
    //
    //     return View(post);
    // }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "CanEditPost")]
    public async Task<IActionResult> Edit(int id, PostUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _postService.UpdatePostAsync(id, dto, userId);

        return RedirectToAction(nameof(Index));
    }

	[HttpPost]
	[ValidateAntiForgeryToken]
	[Authorize(Policy = "CanEditPost")]
	public async Task<IActionResult> Delete(int id)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		try
		{
			await _postService.DeletePostAsync(id, userId);
			return RedirectToAction(nameof(Index));
		}
		catch (UnauthorizedAccessException)
		{
			return Forbid();
		}
		catch (ArgumentException)
		{
			return NotFound();
		}
	}
}
