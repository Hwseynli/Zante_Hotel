using System;
namespace Zante_Hotel.Models
{
	public class HotelService : BaseEntity
    {
        public Guid ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
    }
}

