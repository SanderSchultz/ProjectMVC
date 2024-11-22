using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
				Email = "admin@admin.com",
				UserName = "admin@admin.com",
				NormalizedEmail = "ADMIN@ADMIN.COM",
				NormalizedUserName = "ADMIN@ADMIN.COM",
				Name = "Admin", 
				ProfilePicture = "/images/admin.png",
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var janeUser = new ApplicationUser 
			{
				Id = "2", 
				Email = "jane@jane.com",
				UserName = "jane@jane.com",
				NormalizedEmail = "JANE@JANE.COM",
				NormalizedUserName = "JANE@JANE.COM",
				Name = "Jane",
				ProfilePicture = "/images/jane.jpeg",
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var hasher = new PasswordHasher<ApplicationUser>();
			adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin");
			janeUser.PasswordHash = hasher.HashPassword(janeUser, "jane");

			modelBuilder.Entity<ApplicationUser>().HasData(adminUser, janeUser);

            // Seed initial data for Posts
            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Title = "First Post",
                    ImageUrl = "/images/sunset.webp",
                    Created = DateTime.UtcNow,
                    LikesCount = 0,
                    UserId = adminUser.Id
                },
                new Post
                {
                    Id = 2,
                    Title = "Second Post",
                    ImageUrl = "/images/cat.webp",
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
                    Content = "Nice post, looking forward to more!",
                    PostId = 1,
                    Created = DateTime.UtcNow,
                    UserId = janeUser.Id
                },
                new Comment
                {
                    Id = 2,
                    Content = "Thank you!",
                    PostId = 1,
                    Created = DateTime.UtcNow,
                    UserId = adminUser.Id
                }
            );
		}
    }
}
