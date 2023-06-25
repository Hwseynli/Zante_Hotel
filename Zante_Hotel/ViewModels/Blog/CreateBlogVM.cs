namespace Zante_Hotel.ViewModels
{
	public class CreateBlogVM
	{
        [Required, MinLength(3), MaxLength(55)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(100)]
        public string SubTitle { get; set; }
        public string AuthorId { get; set; }
        public IFormFile Photo { get; set; }
        [Required,MinLength(3),MaxLength(2000)]
        public string Description { get; set; }
        public ICollection<Comment> Comments { get; set; }
        [Required]
        public ICollection<Guid> TagIds { get; set; }
    }
}

