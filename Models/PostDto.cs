namespace ProjectMVC.Models
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int LikesCount { get; set; }
        public string User { get; set; } = string.Empty;
		public string ProfilePicture { get; set; } = string.Empty;
		public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }

    public class PostCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
    }

    public class PostUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
