namespace Zante_Hotel.Models
{
	public class Room:BaseNameableEntity
	{
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
        public bool IsReservation { get; set; } = false;
        public ICollection<Reservation> ReservationsDate { get; set; }
        public ICollection<RoomImage> Images { get; set; }
        [Required]
        public string Description { get; set; }
    }
}

