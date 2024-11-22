using ProjectMVC.Models;

public interface IPostService
{
    Task<List<PostDto>> GetAllPostsAsync(string currentUserId, bool isAdmin);
    Task<PostUpdateDto> GetPostForEditAsync(int id, string userId);
    Task CreatePostAsync(PostCreateDto dto, string userId);
    Task UpdatePostAsync(int id, PostUpdateDto dto, string userId);
    Task DeletePostAsync(int id, string userId);
}
