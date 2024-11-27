using ProjectMVC.Models;

public interface ILikeService
{
    Task<Result> ToggleLikeAsync(int postId, string userId);
}
