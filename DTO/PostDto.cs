namespace ProjectMVC.DTO
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageFile { get; set; } = string.Empty;
        public int LikesCount { get; set; }
        public DateTime Created { get; set; }
        public string User { get; set; } = string.Empty;
		public string ProfilePicture { get; set; } = string.Empty;
		public bool CanChangePost { get; set; }
		public bool IsLikedByUser { get; set; }
		public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }

    public class PostCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
        public string User { get; set; } = string.Empty;
    }

    public class PostUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
    }
}
