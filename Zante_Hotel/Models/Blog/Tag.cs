namespace Zante_Hotel.Models
{
	public class Tag:BaseNameableEntity
	{
		public ICollection<BlogTag> Blogs { get; set; }
	}
}

