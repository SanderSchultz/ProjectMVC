// File: Repositories/PostRepository.cs
using Microsoft.EntityFrameworkCore;
using ProjectMVC.Data;
using ProjectMVC.Models;

namespace ProjectMVC.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PostDto>> GetAllPostsAsync(string currentUserId, bool isAdmin)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
				.OrderByDescending(post => post.Created)
                .Select(post => new PostDto
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
                        Content = c.Content
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
