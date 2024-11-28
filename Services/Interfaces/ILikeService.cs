using ProjectMVC.Models;

namespace ProjectMVC.Services.Interfaces
{
	public interface ILikeService
	{
		Task<Result> ToggleLikeAsync(int postId, string userId);
	}
}
