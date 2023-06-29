using System;
namespace Zante_Hotel.Areas.AppAdmin.Models
{
	public class Hotel : BaseNameableEntity
    {
        [Required, MinLength(3), MaxLength(200)]
        public string Logo { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string Type { get; set; }
        [Required, MinLength(3), MaxLength(2000)]
        public string MapLink { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public byte Rating { get; set; }
        [Required, StringLength(1000)]
        public string Description { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,StringLength(100)]
        public string WebSite { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<HotelService> Services { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
        public ICollection<HotelComment> Comments { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public Spa Spa { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}

