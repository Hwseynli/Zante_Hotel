namespace Zante_Hotel.Models
{
	public class BlogTag:BaseEntity
	{
		[Required]
		public Guid BlogId { get; set; }
		public Blog Blog { get; set; }
		[Required]
		public int TagId { get; set; }
		public Tag Tag { get; set; }

	}
}

