using ProjectMVC.Models;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.DAL.Entities;
using ProjectMVC.Services.Interfaces;

namespace ProjectMVC.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
			_commentRepository = commentRepository;
        }

        public async Task<Result> CreateCommentAsync(int postId, string content, string userId)
        {
            var post = await _commentRepository.GetPostByIdAsync(postId);
            if (post == null){
				return Result.Failure("Post no longer exist");
			}

            var user = await _commentRepository.GetUserByIdAsync(userId);
            if (user == null)
			{
				return Result.Failure("You need to be signed in to comment");
			}

            var newComment = new Comment
            {
                Content = content,
                PostId = postId,
                Created = DateTime.UtcNow,
                User = user
            };

            await _commentRepository.AddCommentAsync(newComment);
			return Result.Success("Comment added successfully");
        }

        public async Task<Result> DeleteCommentAsync(int id, string userId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null || comment.User.Id != userId)
			{
				return Result.Failure("You are not authorized to edit this post");
			}

            await _commentRepository.DeleteCommentAsync(comment);
			return Result.Success("Comment deleted successfully");
        }
    }
}
