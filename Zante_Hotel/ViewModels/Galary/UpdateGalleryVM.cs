using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateGalleryVM
	{
		public IFormFile Photo { get; set; }
		public string Url { get; set; }
		public Guid HotelId { get; set; }
	}
}

