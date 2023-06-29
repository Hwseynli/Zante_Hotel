namespace Zante_Hotel.Models
{
	public class Blog:BaseNameableEntity
	{
        [Required,MinLength(3),MaxLength(100)]
        public string SubTitle { get; set; }
        public DateTime CreateOn { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public AppUser Author { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<BlogTag> Tags { get; set; }
        [Required]
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
    }
}

