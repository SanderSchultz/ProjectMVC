using ProjectMVC.Models;
using ProjectMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectMVC.Services
{
    public interface ICommentService
    {
        Task<Comment> CreateCommentAsync(int postId, string content, string userId);
        Task DeleteCommentAsync(int id, string userId);
    }

    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(int postId, string content, string userId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
                throw new ArgumentException("Post not found", nameof(postId));

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found", nameof(userId));

            var newComment = new Comment
            {
                Content = content,
                PostId = postId,
                Created = DateTime.UtcNow,
                User = user
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return newComment;
        }

        public async Task DeleteCommentAsync(int id, string userId)
        {
            var comment = await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
                throw new ArgumentException("Comment not found", nameof(id));

            if (comment.User.Id != userId)
                throw new UnauthorizedAccessException("User is not authorized to delete this comment");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
