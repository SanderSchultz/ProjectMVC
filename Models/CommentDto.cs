namespace ProjectMVC.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public string User { get; set; } = string.Empty;
    }
}
