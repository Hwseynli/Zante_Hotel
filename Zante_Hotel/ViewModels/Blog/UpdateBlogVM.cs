using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateBlogVM
	{
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public string AuthorId { get; set; }
        public IFormFile Photo { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public Guid HotelId { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<BlogTag> Tags { get; set; }
        public ICollection<Guid> TagIds { get; set; }

    }
}

