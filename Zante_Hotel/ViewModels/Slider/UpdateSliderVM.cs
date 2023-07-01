namespace Zante_Hotel.ViewModels
{
	public class UpdateSliderVM
	{
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(35)]
        public string SubTitle { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        [StringLength(25)]
        public string ButtonTitle { get; set; }
        public IFormFile Photo { get; set; }
    }
}

