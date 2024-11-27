using ProjectMVC.Models;

public interface ILikeRepository
{
    Task<Like> GetLikeAsync(int postId, string userId);
    Task AddLikeAsync(Like like);
    Task RemoveLikeAsync(Like like);
    Task SaveChangesAsync();
}
