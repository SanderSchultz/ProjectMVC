using ProjectMVC.Models;

public interface IPostRepository
{
    Task<List<PostDto>> GetAllPostsAsync(string currentUserId, bool isAdmin);
    Task<Post> GetPostByIdAsync(int id);
    Task AddPostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(Post post);
}
