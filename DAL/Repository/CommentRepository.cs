using Microsoft.EntityFrameworkCore;
using ProjectMVC.DAL.Entities;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.DAL.Repository;

namespace ProjectMVC.DAL.Repository
{
	public class CommentRepository : ICommentRepository
	{
		private readonly ApplicationDbContext _context;

		public CommentRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Comment?> GetCommentByIdAsync(int id)
		{
			return await _context.Comments
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Post?> GetPostByIdAsync(int postId)
		{
			return await _context.Posts.FindAsync(postId);
		}

		public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
		{
			return await _context.Users.FindAsync(userId);
		}

		public async Task AddCommentAsync(Comment comment)
		{
			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCommentAsync(Comment comment)
		{
			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();
		}
	}
}
