using System;
namespace Zante_Hotel.ViewModels
{
	public class CreateBlogVM
	{
        [Required, MinLength(3), MaxLength(25)]
        public string Name { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
        public IFormFile Photo { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}

