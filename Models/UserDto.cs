// DTOs/UserDto.cs
namespace ProjectMVC.Models
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;
    }
}
