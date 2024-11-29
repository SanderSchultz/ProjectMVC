using Microsoft.EntityFrameworkCore;
using ProjectMVC.DAL.Entities;
using ProjectMVC.DAL;
using ProjectMVC.DAL.Repository.Interfaces;

namespace ProjectMVC.Repository
{

public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext _context;

    public PostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .ThenInclude(c => c.User)
            .OrderByDescending(post => post.Created)
            .ToListAsync();
    }

	public async Task<List<int>> GetLikedPostIdsAsync(string userId)
	{
		return await _context.Likes
			.Where(like => like.User.Id == userId)
			.Select(like => like.PostId)
			.ToListAsync();
	}

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddPostAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePostAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePostAsync(Post post)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }
}
}
