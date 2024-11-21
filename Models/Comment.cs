using System;

namespace ProjectMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int PostId { get; set; } // Foreign key
        public Post Post { get; set; } = null!; // Navigation property
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty; // Author of the comment
        public ApplicationUser User { get; set; } = null!; // Author of the comment
    }
}
