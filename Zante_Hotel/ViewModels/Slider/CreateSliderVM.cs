namespace Zante_Hotel.ViewModels
{
	public class CreateSliderVM
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
        [MinLength(3)]
        [MaxLength(25)]
        public string ButtonTitle { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string VideoUrl { get; set; }
    }
}

