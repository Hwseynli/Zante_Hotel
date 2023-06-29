namespace Zante_Hotel.ViewModels
{
	public class UpdateSliderVM
	{
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public string ButtonTitle { get; set; }
        public IFormFile Photo { get; set; }
    }
}

