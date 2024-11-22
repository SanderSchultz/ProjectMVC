using Microsoft.AspNetCore.Identity;

namespace ProjectMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
		public string ProfilePicture { get; set; } = string.Empty;
    }
}
