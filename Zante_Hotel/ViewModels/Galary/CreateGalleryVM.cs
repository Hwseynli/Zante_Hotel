namespace Zante_Hotel.ViewModels
{
	public class CreateGalleryVM
	{
		[Required]
		public IFormFile Photo { get; set; }
		[Required]
		public Guid HotelId { get; set; }
	}
}

