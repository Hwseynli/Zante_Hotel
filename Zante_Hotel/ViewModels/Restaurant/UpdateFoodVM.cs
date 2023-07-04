namespace Zante_Hotel.ViewModels
{
	public class UpdateFoodVM
	{
        public string ImgUrl { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string About { get; set; }
        public IFormFile Photo { get; set; }
        public decimal Price { get; set; }
    }
}

