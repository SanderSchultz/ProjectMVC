using ProjectMVC.Models;
using ProjectMVC.DAL.Entities;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.Services.Interfaces;
using System.Security.Claims;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IPostRepository _postRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LikeService(ILikeRepository likeRepository, IPostRepository postRepository, IHttpContextAccessor httpContextAccessor)
    {
        _likeRepository = likeRepository;
        _postRepository = postRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> ToggleLikeAsync(int postId, string userId)
    {

		if(userId == null)
		{
			return Result.Failure("You need to be logged in to like posts");
		}

        var post = await _postRepository.GetPostByIdAsync(postId);
        if (post == null)
        {
            return Result.Failure("Post does not exist");
        }

        var existingLike = await _likeRepository.GetLikeAsync(postId, userId);

        if (existingLike != null)
        {
            _likeRepository.RemoveLikeAsync(existingLike);
			post.LikesCount--;
        }
        else
        {
            var like = new Like
            {
                PostId = postId,
                UserId = userId,
            };
            await _likeRepository.AddLikeAsync(like);
            post.LikesCount++;
        }

        await _likeRepository.SaveChangesAsync();

		if(existingLike != null){
			return Result.Success("Unliked post successfully");
		}

		return Result.Success("Liked post successfully");

    }
}
