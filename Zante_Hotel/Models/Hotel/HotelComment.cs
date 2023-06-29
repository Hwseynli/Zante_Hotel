using System;
namespace Zante_Hotel.Models
{
	public class HotelComment : BaseEntity
    {
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
        public Guid CommentId { get; set; }
        [ForeignKey("HotelId")]
        public Comment Comment { get; set; }
    }
}

