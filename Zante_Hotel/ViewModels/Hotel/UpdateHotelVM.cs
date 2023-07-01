using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateHotelVM
	{
        [StringLength(25)]
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public IFormFile Logo { get; set; }
        public string Type { get; set; }
        [DataType(DataType.Url)]
        public string MapLink { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        public byte Rating { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(100)]
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

