namespace Zante_Hotel.Models
{
	public class Comment:BaseEntity
	{
		[Required, MinLength(1), MaxLength(500)]
		public string Text { get; set; }
		public string UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual AppUser User { get; set; }
		public ICollection<Reply> Replies { get; set; }
		[ScaffoldColumn(false)]
		public DateTime CreateOn { get; set; }
	}
}

