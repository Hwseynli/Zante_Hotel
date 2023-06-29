using System;
namespace Zante_Hotel.Areas.AppAdmin.Models
{
	public class Spa: BaseNameableEntity
    {
        [Required, MinLength(3), MaxLength(200)]
        public string SubTitle { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string Title { get; set; }
        [Required, MinLength(3), MaxLength(20000)]
        public string Decription { get; set; }
        [Required]
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
        public ICollection<SpaImage> Images { get; set; }
    }
    public class SpaImage : BaseEntity
    {
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public bool? IsPrimary { get; set; }
        public Guid SpaId { get; set; }
        [ForeignKey("SpaId")]
        public Spa Spa { get; set; }
    }
}

