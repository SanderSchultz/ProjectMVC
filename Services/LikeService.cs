using ProjectMVC.Models;
using ProjectMVC.DAL.Entities;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.Services.Interfaces;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IPostRepository _postRepository;

    public LikeService(ILikeRepository likeRepository, IPostRepository postRepository)
    {
        _likeRepository = likeRepository;
        _postRepository = postRepository;
    }

    public async Task<Result> ToggleLikeAsync(int postId, string userId)
    {
		
		if(string.IsNullOrEmpty(userId))
		{
			return Result.Failure("You need to be logged in to like posts");
		}

        /* Check if the post exists */
        var post = await _postRepository.GetPostByIdAsync(postId);
        if (post == null)
        {
            return Result.Failure("Post does not exist");
        }

        /* Check if the user has already liked the post */
        var existingLike = await _likeRepository.GetLikeAsync(postId, userId);

        if (existingLike != null)
        {
			/* Unlike */
            _likeRepository.RemoveLikeAsync(existingLike);
			post.LikesCount--;
        }
        else
        {
            /* Like */
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
