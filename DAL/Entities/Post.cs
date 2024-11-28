
namespace ProjectMVC.DAL.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageFile { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public int LikesCount { get; set; } = 0;
		public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
