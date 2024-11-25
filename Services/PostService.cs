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
            return await _postRepository.GetAllPostsAsync(currentUserId, isAdmin);
        }

        public async Task CreatePostAsync(PostCreateDto dto, string userId)
        {
			string imagePath = string.Empty;

			// Handle file saving
			if (dto.ImageFile != null && dto.ImageFile.Length > 0)
			{
				// Generate unique filename
				var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);

				// File path in wwwroot
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

				// Save the file
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await dto.ImageFile.CopyToAsync(stream);
				}

				// Relative path to save in database
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
        }

        public async Task UpdatePostAsync(int id, PostUpdateDto dto, string userId)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null || post.UserId != userId)
                throw new UnauthorizedAccessException("You are not authorized to edit this post.");

            post.Title = dto.Title;
            // post.ImageFile = dto.ImageFile;

            await _postRepository.UpdatePostAsync(post);
        }

		public async Task DeletePostAsync(int id, string userId)
		{
			var post = await _postRepository.GetPostByIdAsync(id);
			if (post == null)
				throw new ArgumentException("Post not found", nameof(id));

			if (post.UserId != userId)
				throw new UnauthorizedAccessException("User is not authorized to delete this post");

			await _postRepository.DeletePostAsync(post);
		}

    }
}
