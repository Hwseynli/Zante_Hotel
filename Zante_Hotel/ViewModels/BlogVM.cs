using System;
namespace Zante_Hotel.ViewModels
{
	public class BlogVM
	{
		public Blog Blog { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<Blog> Blogs { get; set; }
		public ICollection<Reply> Replies { get; set; }
	}
}

