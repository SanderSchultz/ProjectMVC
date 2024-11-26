using ProjectMVC.Models;

public interface ICommentRepository
{
    Task<Comment?> GetCommentByIdAsync(int id);
    Task<Post?> GetPostByIdAsync(int postId);
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task AddCommentAsync(Comment comment);
    Task DeleteCommentAsync(Comment comment);
}