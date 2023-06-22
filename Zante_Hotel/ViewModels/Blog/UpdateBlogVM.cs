using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateBlogVM
	{
        public string Name { get; set; }
        public Guid AuthorId { get; set; }
        public IFormFile Photo { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}

