using ProjectMVC.Models;

public interface IPostService
{
    Task<List<PostDto>> GetAllPostsAsync(string currentUserId, bool isAdmin);
    Task<Result> CreatePostAsync(PostCreateDto dto, string userId);
    Task<Result> UpdatePostAsync(int id, PostUpdateDto dto, string userId);
    Task<Result> DeletePostAsync(int id, string userId);
}
