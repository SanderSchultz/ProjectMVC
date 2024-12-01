using ProjectMVC.Models;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.DAL.Entities;
using ProjectMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProjectMVC.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(ICommentRepository commentRepository, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
			_commentRepository = commentRepository;
			_authorizationService = authorizationService;
			_httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> CreateCommentAsync(int postId, string content)
        {
            var post = await _commentRepository.GetPostByIdAsync(postId);
            if (post == null)
			{
				return Result.Failure("Post no longer exist");
			}

			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if(userId == null)
			{
				return Result.Failure("You need to be signed in to add comments");
			}

            var newComment = new Comment
            {
                Content = content,
                PostId = postId,
                Created = DateTime.UtcNow,
                UserId = userId
            };

            await _commentRepository.AddCommentAsync(newComment);
			return Result.Success("Comment added successfully");
        }

        public async Task<Result> DeleteCommentAsync(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
			if(comment == null)
			{
				return Result.Failure("This comment does not exist");
			}

			var user = _httpContextAccessor.HttpContext?.User;

			if(user == null)
			{
				return Result.Failure("You need to be signed in to delete comments");
			}

			var authorizationResult = await _authorizationService.AuthorizeAsync(user, comment, "CanEdit");

			if(!authorizationResult.Succeeded)
			{
				return Result.Failure("You are not authorized to delete this comment");

			}

            await _commentRepository.DeleteCommentAsync(comment);
			return Result.Success("Comment deleted successfully");
        }
    }
}
