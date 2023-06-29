using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateHotelVM
	{
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public IFormFile Logo { get; set; }
        public string Type { get; set; }
        public string MapLink { get; set; }
        public string Address { get; set; }
        public byte Rating { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Guid> ServiceIds { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
        public ICollection<HotelComment> Comments { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public Spa Spa { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}

