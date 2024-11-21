using System;

namespace ProjectMVC.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int PostId { get; set; } // Foreign key
        public Post Post { get; set; } = null!; // Navigation property
        public ApplicationUser User { get; set; } = null!; // User who liked the post
    }
}
