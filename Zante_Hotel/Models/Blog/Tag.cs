namespace Zante_Hotel.Models
{
	public class Tag:BaseNameableEntity
	{
		public ICollection<Blog> Blogs { get; set; }
	}
}

