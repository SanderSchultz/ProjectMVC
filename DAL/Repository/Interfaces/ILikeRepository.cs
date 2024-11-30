using ProjectMVC.DAL.Entities;

namespace ProjectMVC.DAL.Repository.Interfaces
{
	public interface ILikeRepository
	{
		Task<Like?> GetLikeAsync(int postId, string userId);
		Task AddLikeAsync(Like like);
		void RemoveLikeAsync(Like like);
		Task SaveChangesAsync();
	}
}
