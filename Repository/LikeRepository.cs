using Microsoft.EntityFrameworkCore;
using ProjectMVC.Data;
using ProjectMVC.Models;

public class LikeRepository : ILikeRepository
{
    private readonly ApplicationDbContext _context;

    public LikeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Like> GetLikeAsync(int postId, string userId)
    {
        return await _context.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
    }

    public async Task AddLikeAsync(Like like)
    {
        await _context.Likes.AddAsync(like);
    }

    public async Task RemoveLikeAsync(Like like)
    {
        _context.Likes.Remove(like);
    }

    public async Task SaveChangesAsync()
    {
		await _context.SaveChangesAsync();
    }
}
