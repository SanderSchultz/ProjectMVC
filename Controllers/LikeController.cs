using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMVC.Data;
using ProjectMVC.Models;
using System.Threading.Tasks;

namespace ProjectMVC.Controllers
{
    public class LikeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LikeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: Like/Toggle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user == null)
                return Unauthorized();

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.User.Id == user.Id);

            if (existingLike != null)
            {
                // Unlike
                _context.Likes.Remove(existingLike);
                post.LikesCount--;
            }
            else
            {
                // Like
                var newLike = new Like
                {
                    PostId = postId,
                    Post = post,
                    User = user
                };
                _context.Likes.Add(newLike);
                post.LikesCount++;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Post");
        }
    }
}
