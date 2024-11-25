using ProjectMVC.Models;
using ProjectMVC.Repositories;

namespace ProjectMVC.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
			_commentRepository = commentRepository;
        }

        public async Task<Comment> CreateCommentAsync(int postId, string content, string userId)
        {
            var post = await _commentRepository.GetPostByIdAsync(postId);
            if (post == null)
                throw new ArgumentException("Post not found", nameof(postId));

            var user = await _commentRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found", nameof(userId));

            var newComment = new Comment
            {
                Content = content,
                PostId = postId,
                Created = DateTime.UtcNow,
                User = user
            };

            await _commentRepository.AddCommentAsync(newComment);
            return newComment;
        }

        public async Task DeleteCommentAsync(int id, string userId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
                throw new ArgumentException("Comment not found", nameof(id));

            if (comment.User.Id != userId)
                throw new UnauthorizedAccessException("User is not authorized to delete this comment");

            await _commentRepository.DeleteCommentAsync(comment);
        }
    }
}
