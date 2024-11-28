
namespace ProjectMVC.DAL.Entities
{
	public class Like
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
