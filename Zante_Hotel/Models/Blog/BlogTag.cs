namespace Zante_Hotel.Models
{
	public class BlogTag:BaseEntity
	{
		[Required]
		public Guid BlogId { get; set; }
		[ForeignKey("BlogId")]
        public Blog Blog { get; set; }
		[Required]
		public Guid TagId { get; set; }
		[ForeignKey("TagId")]
        public Tag Tag { get; set; }

	}
}

