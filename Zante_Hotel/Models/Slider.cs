namespace Zante_Hotel.Models
{
	public class Slider:BaseEntity
	{
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(35)]
        public string SubTitle { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string VideoUrl { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string ButtonTitle { get; set; }
    }
}

