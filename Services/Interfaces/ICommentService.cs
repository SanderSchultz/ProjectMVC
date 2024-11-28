using ProjectMVC.Models;

namespace ProjectMVC.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Result> CreateCommentAsync(int postId, string content, string userId);
        Task<Result> DeleteCommentAsync(int id, string userId);
    }
}
