using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProjectMVC.Data;
using ProjectMVC.Models;

namespace ProjectMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
			_userManager = userManager;
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int postId, string content)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
                return NotFound();

			var user = await _userManager.GetUserAsync(User);

			if(user == null){
				return RedirectToAction("Login", "Account");
			}

            var newComment = new Comment
            {
                Content = content,
                PostId = postId,
                Created = DateTime.UtcNow,
                User = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name)
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Post");
        }

        // POST: Comment/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Post");
        }
    }
}
