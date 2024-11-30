using ProjectMVC.DAL.Entities;

namespace ProjectMVC.DAL.Repository.Interfaces
{
	public interface IPostRepository
	{
		Task<List<Post>> GetAllPostsAsync();
		Task<List<int>> GetLikedPostIdsAsync(string userId);
		Task<Post?> GetPostByIdAsync(int id);
		Task AddPostAsync(Post post);
		Task UpdatePostAsync(Post post);
		Task DeletePostAsync(Post post);
	}
}
