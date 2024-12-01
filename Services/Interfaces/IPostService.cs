using ProjectMVC.Models;
using ProjectMVC.DTO;

namespace ProjectMVC.Services.Interfaces
{
public interface IPostService
	{
		Task<List<PostDto>> GetAllPostsAsync();
		Task<Result> CreatePostAsync(PostCreateDto dto);
		Task<Result> UpdatePostAsync(int id, PostUpdateDto dto);
		Task<Result> DeletePostAsync(int id);
	}
}
