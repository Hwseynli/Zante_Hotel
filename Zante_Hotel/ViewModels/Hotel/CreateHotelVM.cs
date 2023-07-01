using System;
namespace Zante_Hotel.ViewModels
{
	public class CreateHotelVM
	{
        [Required, MinLength(3), MaxLength(25)]
        public string Name { get; set; }
        [Required]
        public IFormFile Logo { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string Type { get; set; }
        [Required, MinLength(3), DataType(DataType.Url)]
        public string MapLink { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public byte Rating { get; set; }
        [Required, StringLength(1000)]
        public string Description { get; set; }
        [Required, DataType(DataType.PhoneNumber),StringLength(15)]
        public string PhoneNumber { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, StringLength(100)]
        public string WebSite { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public List<Guid> ServiceIds { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
        public ICollection<HotelComment> Comments { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public Spa Spa { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}

