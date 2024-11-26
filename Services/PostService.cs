// File: Services/PostService.cs
using ProjectMVC.Models;
using ProjectMVC.Repositories;

namespace ProjectMVC.Services
{

	public class PostService : IPostService
	{
		private readonly IPostRepository _postRepository;

		public PostService(IPostRepository postRepository)
		{
			_postRepository = postRepository;
		}

		public async Task<List<PostDto>> GetAllPostsAsync(string currentUserId, bool isAdmin)
		{
			// Fetch raw posts from the repository
			var posts = await _postRepository.GetAllPostsAsync();

			// Map to DTOs and apply business logic
			return posts.Select(post => new PostDto
			{
				Id = post.Id,
				Title = post.Title,
				ImageFile = post.ImageFile,
				LikesCount = post.LikesCount,
				User = post.User.Name,
				ProfilePicture = post.User.ProfilePicture,
				CanChangePost = isAdmin || post.User.Id == currentUserId,
				Comments = post.Comments
					.OrderByDescending(c => c.Created)
					.Select(c => new CommentDto
					{
						User = c.User.Name,
						ProfilePicture = c.User.ProfilePicture,
						Content = c.Content,
						CanEdit = c.User.Id == currentUserId
					}).ToList()
			}).ToList();
		}

		public async Task<Result> CreatePostAsync(PostCreateDto dto, string userId)
		{
			if(string.IsNullOrEmpty(userId))
			{
				return Result.Failure("You need to be logged in to create posts");
			}

			string imagePath = string.Empty;

			if (dto.ImageFile != null && dto.ImageFile.Length > 0)
			{
				var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

				Directory.CreateDirectory(Path.GetDirectoryName(filePath));

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await dto.ImageFile.CopyToAsync(stream);
				}

				imagePath = $"/images/{fileName}";
			}

			var newPost = new Post
			{
				Title = dto.Title,
				ImageFile = imagePath,
				UserId = userId,
				Created = DateTime.UtcNow
			};

			await _postRepository.AddPostAsync(newPost);
			return Result.Success("Post created successfully");
		}

		public async Task<Result> UpdatePostAsync(int id, PostUpdateDto dto, string userId)
		{
			var post = await _postRepository.GetPostByIdAsync(id);
			if (post == null || post.UserId != userId)
			{
				return Result.Failure("You are not authorized to edit this post");
			}

			post.Title = dto.Title;

			await _postRepository.UpdatePostAsync(post);
			return Result.Success("Post edited successfully");
		}

		public async Task<Result> DeletePostAsync(int id, string userId)
		{
			var post = await _postRepository.GetPostByIdAsync(id);
			if (post == null || post.UserId != userId)
			{
				return Result.Failure("You are not authorized to delete this post");
			}

			await _postRepository.DeletePostAsync(post);
			return Result.Success("Post deleted successfully");
		}
	}
}
