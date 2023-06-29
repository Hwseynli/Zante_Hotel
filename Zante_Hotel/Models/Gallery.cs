namespace Zante_Hotel.Models
{
	public class Gallery:BaseEntity
	{
        [Required, MinLength(5), MaxLength(200)]
        public string Url { get; set; }
        [Required]
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
    }
}

