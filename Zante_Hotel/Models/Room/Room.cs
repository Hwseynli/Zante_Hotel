namespace Zante_Hotel.Models
{
	public class Room:BaseEntity
	{
        [Required,MinLength(1),MaxLength(20)]
        public string Number { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
        public ICollection<RoomServices> Services { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        public Guid ViewId { get; set; }
        [ForeignKey("ViewId")]
        public View View { get; set; }
        public ICollection<Reservation> ReservationsDate { get; set; }
        public ICollection<RoomImage> Images { get; set; }
        [Required,StringLength(2000)]
        public string Description { get; set; }
        [Required]
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
    }
}

