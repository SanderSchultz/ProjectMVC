using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectMVC.Models;

namespace ProjectMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			var adminUser = new ApplicationUser { 
				Id= "1", 
				UserName = "admin@admin.com",
				Name = "Admin", 
				// ProfilePicture = "/images/admin.png"
			};

			var janeUser = new ApplicationUser 
			{
				Id = "2", 
				UserName = "jane@jane.com", 
				Name = "Jane",
				// ProfilePicture = "/images/admin.png"
			};

			modelBuilder.Entity<ApplicationUser>().HasData(adminUser, janeUser);

            // Seed initial data for Posts
            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Title = "First Post",
                    ImageUrl = "/images/arrow.jpg",
                    Created = DateTime.UtcNow,
                    LikesCount = 0,
                    UserId = adminUser.Id
                },
                new Post
                {
                    Id = 2,
                    Title = "Second Post",
                    ImageUrl = "/images/arrow.jpg",
                    Created = DateTime.UtcNow,
                    LikesCount = 0,
                    UserId = janeUser.Id
                }
            );

            // Seed initial data for Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    Content = "This is a comment on the first post.",
                    PostId = 1,
                    Created = DateTime.UtcNow,
                    UserId = janeUser.Id
                },
                new Comment
                {
                    Id = 2,
                    Content = "Nice post, looking forward to more!",
                    PostId = 1,
                    Created = DateTime.UtcNow,
                    UserId = janeUser.Id
                }
            );
		}
    }
}
