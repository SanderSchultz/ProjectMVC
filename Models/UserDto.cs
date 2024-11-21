// DTOs/UserDto.cs
namespace ProjectMVC.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty; // From IdentityUser.Id
        public string UserName { get; set; } = string.Empty; // From IdentityUser.UserName
        public string Email { get; set; } = string.Empty; // From IdentityUser.Email
        public string Name { get; set; } = string.Empty; // From ApplicationUser.Name
        // Add other properties as needed, but exclude sensitive ones
    }
}
