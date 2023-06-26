namespace Zante_Hotel.ViewModels
{
	public class CreateRoomVM
	{
        [Required,MinLength(1),MaxLength(20)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
        public ICollection<RoomServices> RoomServices { get; set; }
        public ICollection<RoomImage> Images { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid ViewId { get; set; }
        public bool IsReservation { get; set; } = false;
        public ICollection<Reservation> ReservationsDate { get; set; }
       
    }
}

