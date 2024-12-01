using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProjectMVC.DAL.Entities;

namespace ProjectMVC.DAL
{
    public static class DBInit
    {
        public static void Seed(ModelBuilder modelBuilder)
        {

			var hasher = new PasswordHasher<ApplicationUser>();

			// Create the Admin role
			var adminRole = new IdentityRole
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Admin",
				NormalizedName = "ADMIN"
			};

			var adminUser = new ApplicationUser { 
				Id = Guid.NewGuid().ToString(),
				Email = "admin@admin.com",
				UserName = "admin@admin.com",
				NormalizedEmail = "ADMIN@ADMIN.COM",
				NormalizedUserName = "ADMIN@ADMIN.COM",
				Name = "Admin", 
				ProfilePicture = "/images/admin.jpg",
				SecurityStamp = Guid.NewGuid().ToString()
			};

			adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin");

			// Associate the user with the Admin role
			var adminUserRole = new IdentityUserRole<string>
			{
				UserId = adminUser.Id,
				RoleId = adminRole.Id
			};

			modelBuilder.Entity<IdentityRole>().HasData(adminRole);
			modelBuilder.Entity<ApplicationUser>().HasData(adminUser);
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminUserRole);

			var janeUser = new ApplicationUser 
			{
				Email = "jane@jane.com",
				UserName = "jane@jane.com",
				NormalizedEmail = "JANE@JANE.COM",
				NormalizedUserName = "JANE@JANE.COM",
				Name = "Jane Dawson",
				ProfilePicture = "/images/jane.jpeg",
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var patrickUser = new ApplicationUser 
			{
				Email = "patrick@patrick.com",
				UserName = "patrick@patrick.com",
				NormalizedEmail = "PATRICK@PATRICK.COM",
				NormalizedUserName = "PATRICK@PATRICK.COM",
				Name = "Patrick Johnson",
				ProfilePicture = "/images/patrick.jpg",
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var annaUser = new ApplicationUser 
			{
				Email = "anna@anna.com",
				UserName = "anna@anna.com",
				NormalizedEmail = "ANNA@ANNA.COM",
				NormalizedUserName = "ANNA@ANNA.COM",
				Name = "Anna Persson",
				ProfilePicture = "/images/anna.jpg",
				SecurityStamp = Guid.NewGuid().ToString()
			};


			janeUser.PasswordHash = hasher.HashPassword(janeUser, "jane");
			patrickUser.PasswordHash = hasher.HashPassword(patrickUser, "patrick");
			annaUser.PasswordHash = hasher.HashPassword(patrickUser, "anna");

			modelBuilder.Entity<ApplicationUser>().HasData(janeUser, patrickUser, annaUser);

            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Title = "First Post",
                    ImageFile = "/images/cat_on_stairs.jpg",
                    Created = new DateTime(2024, 11, 27, 10, 0, 0),
                    LikesCount = 13,
                    UserId = adminUser.Id
                },
                new Post
                {
                    Id = 2,
                    Title = "Slovenia is so beautiful❤️ 🇸🇮",
                    ImageFile = "/images/slovenian_island.jpg",
                    Created = new DateTime(2024, 11, 28, 13, 40, 0),
                    LikesCount = 389,
                    UserId = janeUser.Id
                },
                new Post
                {
                    Id = 3,
                    Title = "Another view from my trip!",
                    ImageFile = "/images/hills.png",
                    Created = new DateTime(2024, 11, 28, 18, 3, 0),
                    LikesCount = 139,
                    UserId = patrickUser.Id
                }
            );

            // Seed initial data for Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    Content = "Nice post, looking forward to more!",
                    PostId = 3,
                    Created = new DateTime(2024, 11, 28, 18, 7, 0),
                    UserId = janeUser.Id,

                },
                new Comment
                {
                    Id = 2,
                    Content = "Incredible photo skills",
                    PostId = 3,
                    Created = new DateTime(2024, 11, 28, 18, 10, 0),
                    UserId = adminUser.Id,

                },
                new Comment
                {
                    Id = 3,
                    Content = "Thanks! There is more to come📍",
                    PostId = 3,
                    Created = new DateTime(2024, 11, 28, 18, 19, 0),
                    UserId = patrickUser.Id,

                },
                new Comment
                {
                    Id = 4,
                    Content = "Wish I could have been there myself, where is this?",
                    PostId = 3,
                    Created = new DateTime(2024, 11, 28, 18, 24, 0),
                    UserId = annaUser.Id,

                },
				new Comment
                {
                    Id = 5,
                    Content = "It's in Mexico 🇲🇽",
                    PostId = 3,
                    Created = new DateTime(2024, 11, 28, 18, 29, 0),
                    UserId = patrickUser.Id,

                },
				new Comment
                {
                    Id = 6,
                    Content = "Mexico looks great!",
                    PostId = 3,
                    Created = new DateTime(2024, 11, 28, 18, 39, 0),
                    UserId = janeUser.Id,

                },
				new Comment
                {
                    Id = 7,
                    Content = "It absolutely is, you should visit some time!",
                    PostId = 3,
                    Created = new DateTime(2024, 11, 28, 18, 42, 0),
                    UserId = patrickUser.Id,

                },
                new Comment
                {
                    Id = 8,
                    Content = "Thank you!",
                    PostId = 1,
                    Created = new DateTime(2024, 11, 28, 18, 56, 0),
                    UserId = adminUser.Id
                }
            );

        }
    }
}
