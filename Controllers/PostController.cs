using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectMVC.Data;
using ProjectMVC.Models;

namespace ProjectMVC.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger<PostController> _logger;

        public PostController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<PostController> logger)
        {
            _context = context;
			_userManager = userManager;
			_logger = logger;

        }

        // GET: Post
        public async Task<IActionResult> Index()
        {

			ViewBag.SuccessMessage = TempData["SuccessMessage"];

            var postDtos = await _context.Posts
				.Include(p => p.User)
				.Include(p => p.Comments)
				.ThenInclude(c => c.User)
                .Select(post => new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    ImageUrl = post.ImageUrl,
                    LikesCount = post.LikesCount,
                    User = post.User.Name,
					ProfilePicture = post.User.ProfilePicture,
					Comments = post.Comments.Select(c => new CommentDto
					{
						User = c.User.Name,
						ProfilePicture = c.User.ProfilePicture,
						Content = c.Content
					}).ToList()
                })
				.ToListAsync();

			if(postDtos == null){
				_logger.LogInformation("No posts");
			} else {
				_logger.LogInformation($"Fetched {postDtos.Count}");
			}

            return View(postDtos);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateDto dto)
        {
			var user = await _userManager.GetUserAsync(User);

			if(user == null){
				return RedirectToAction("Login", "Account");
			}

            if (ModelState.IsValid)
            {
                var newPost = new Post
                {
                    Title = dto.Title,
                    ImageUrl = dto.ImageUrl,
                    User = user,
                    Created = DateTime.UtcNow
                };

                _context.Posts.Add(newPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }

        // GET: Post/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts
				.Include(p => p.User)
				.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

			var user = await _userManager.GetUserAsync(User);

			if(user == null || post.User.Id != user.Id){
				return Unauthorized();
			}

            var updateDto = new PostUpdateDto
            {
                Title = post.Title,
                ImageUrl = post.ImageUrl
            };

            return View(updateDto);
        }

        // POST: Post/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostUpdateDto dto)
        {
            var post = await _context.Posts
				.Include(p => p.User)
				.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

			var user = await _userManager.GetUserAsync(User);

			if(user == null || post.User.Id != user.Id){
				return Unauthorized();
			}

            if (ModelState.IsValid)
            {
                post.Title = dto.Title;
                post.ImageUrl = dto.ImageUrl;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }

        // POST: Post/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts
				.Include(p => p.User)
				.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

			var user = await _userManager.GetUserAsync(User);

			if(user == null || post.User.Id != user.Id){
				return Unauthorized();
			}

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
