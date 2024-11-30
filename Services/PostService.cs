using ProjectMVC.DAL.Entities;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.Services.Interfaces;
using ProjectMVC.DTO;
using ProjectMVC.Models;

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

			var likedPostIds = await _postRepository.GetLikedPostIdsAsync(currentUserId);

			// Map to DTOs and apply business logic
			return posts.Select(post => new PostDto
			{
				Id = post.Id,
				Title = post.Title,
				ImageFile = post.ImageFile,
				LikesCount = post.LikesCount,
				Created = post.Created,
				User = post.User.Name,
				ProfilePicture = post.User.ProfilePicture,
				CanChangePost = isAdmin || post.User.Id == currentUserId,
				IsLikedByUser = likedPostIds.Contains(post.Id),
				Comments = post.Comments
					.OrderBy(c => c.Created)
					.Select(c => new CommentDto
					{
						Id = c.Id,
						User = c.User.Name,
						ProfilePicture = c.User.ProfilePicture,
						Content = c.Content,
						CanEdit = c.User.Id == currentUserId,
						Created = c.Created
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

			string imagePath = string.Empty;

			if (dto.ImageFile != null && dto.ImageFile.Length > 0)
			{

				var deletefilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.ImageFile.TrimStart('/'));
				if (File.Exists(deletefilePath))
				{
					File.Delete(deletefilePath);
					Console.WriteLine($"Deleted image: {deletefilePath}");
				}
				else
				{
					Console.WriteLine($"Image not found: {deletefilePath}");
				}

				var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

				Directory.CreateDirectory(Path.GetDirectoryName(filePath));

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await dto.ImageFile.CopyToAsync(stream);
				}

				imagePath = $"/images/{fileName}";
				post.ImageFile = imagePath;
			}

			if(!string.IsNullOrEmpty(dto.Title))
			{
				post.Title = dto.Title;
			}

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

			if (!string.IsNullOrEmpty(post.ImageFile))
			{
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.ImageFile.TrimStart('/'));
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
					Console.WriteLine($"Deleted image: {filePath}");
				}
				else
				{
					Console.WriteLine($"Image not found: {filePath}");
				}
			}


			await _postRepository.DeletePostAsync(post);
			return Result.Success("Post deleted successfully");
		}
	}
}
