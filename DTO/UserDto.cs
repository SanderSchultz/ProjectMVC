namespace ProjectMVC.DTO
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;
		public string? OldPassword { get; set; }
		public string? ProfilePicture { get; set; }
		public IFormFile? ProfilePictureIForm { get; set; }
    }
}
