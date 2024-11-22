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

        public async Task<PostUpdateDto> GetPostForEditAsync(int id, string userId)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null || post.User.Id != userId)
                return null;

            return new PostUpdateDto
            {
                Title = post.Title,
                ImageUrl = post.ImageUrl
            };
        }

        public async Task CreatePostAsync(PostCreateDto dto, string userId)
        {
            var newPost = new Post
            {
                Title = dto.Title,
                ImageUrl = dto.ImageUrl,
                UserId = userId,
                Created = DateTime.UtcNow
            };

            await _postRepository.AddPostAsync(newPost);
        }

        public async Task UpdatePostAsync(int id, PostUpdateDto dto, string userId)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null || post.User.Id != userId)
                throw new UnauthorizedAccessException("You are not authorized to edit this post.");

            post.Title = dto.Title;
            post.ImageUrl = dto.ImageUrl;

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
