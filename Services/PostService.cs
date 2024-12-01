using ProjectMVC.DAL.Entities;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.Services.Interfaces;
using ProjectMVC.DTO;
using ProjectMVC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProjectMVC.Services
{

	public class PostService : IPostService
	{
		private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<PostService> _logger;

		public PostService(IPostRepository postRepository, IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, ILogger<PostService> logger)
		{
			_postRepository = postRepository;
			_httpContextAccessor = httpContextAccessor;
			_authorizationService = authorizationService;
			_logger = logger;
		}

		public async Task<List<PostDto>> GetAllPostsAsync()
		{

			var user = _httpContextAccessor.HttpContext?.User;
			var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);

			// Fetch raw posts from the repository
			var posts = await _postRepository.GetAllPostsAsync();

			var adminCheck = user != null 
				? await _authorizationService.AuthorizeAsync(user, null, "CanEdit")
				: AuthorizationResult.Failed();

			var isAdmin = adminCheck.Succeeded;
			
			_logger.LogInformation("Admin check result: {IsAdmin}", isAdmin);

			var likedPostIds = userId != null
				? await _postRepository.GetLikedPostIdsAsync(userId)
				: new List<int>();

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
				CanChangePost = isAdmin || post.User.Id == userId,
				IsLikedByUser = likedPostIds.Contains(post.Id),
				Comments = post.Comments
					.OrderBy(c => c.Created)
					.Select(c => new CommentDto
					{
						Id = c.Id,
						User = c.User.Name,
						ProfilePicture = c.User.ProfilePicture,
						Content = c.Content,
						CanEdit = isAdmin || c.User.Id == userId,
						Created = c.Created
					}).ToList()
			}).ToList();
		}

		public async Task<Result> CreatePostAsync(PostCreateDto dto)
		{

			var user = _httpContextAccessor.HttpContext?.User;
			var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);

			if(user == null)
			{
				return Result.Failure("You need to be logged in to create posts");
			}

			if(userId == null)
			{
				return Result.Failure("Missing claims, please relog");
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

		public async Task<Result> UpdatePostAsync(int id, PostUpdateDto dto)
		{

			var post = await _postRepository.GetPostByIdAsync(id);
			if (post == null)
			{
				return Result.Failure("Post does not exist");
			}

			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if(post.UserId != userId)
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

		public async Task<Result> DeletePostAsync(int id)
		{
			var post = await _postRepository.GetPostByIdAsync(id);
			if (post == null)
			{
				return Result.Failure("Post does not exist");
			}

			var user = _httpContextAccessor.HttpContext?.User;
			var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);

			var adminCheck = user != null 
				? await _authorizationService.AuthorizeAsync(user, post, "CanEdit")
				: AuthorizationResult.Failed();

			var isAdmin = adminCheck.Succeeded;

			if(post.UserId != userId && !isAdmin)
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
