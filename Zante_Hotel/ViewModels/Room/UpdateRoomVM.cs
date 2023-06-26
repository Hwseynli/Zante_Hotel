namespace Zante_Hotel.ViewModels
{
	public class UpdateRoomVM
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int NumberOfPeople { get; set; }
        public ICollection<RoomServices> RoomServices { get; set; }
        public ICollection<RoomImage> Images { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ViewId { get; set; }
        public bool IsReservation { get; set; } = false;
        public ICollection<Reservation> ReservationsDate { get; set; }
  
    }
}

