using ProjectMVC.Models;
using ProjectMVC.Data;
using Microsoft.EntityFrameworkCore;

    public interface ICommentService
    {
        Task<Comment> CreateCommentAsync(int postId, string content, string userId);
        Task DeleteCommentAsync(int id, string userId);
    }
