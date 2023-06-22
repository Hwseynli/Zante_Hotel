namespace Zante_Hotel.Models
{
	public class Blog:BaseNameableEntity
	{
		public Guid AuthorId { get; set; }
		public AppUser Author { get; set; }
		[Required]
		public string ImgUrl { get; set; }
		[Required]
		public string Description { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<Tag> Tags { get; set; }
    }
}

