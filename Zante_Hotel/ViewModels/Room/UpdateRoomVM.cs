namespace Zante_Hotel.ViewModels
{
	public class UpdateRoomVM
	{
        [StringLength(20)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int NumberOfPeople { get; set; }
        public IFormFile MainPhoto { get; set; }
        public ICollection<IFormFile> Photos { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ViewId { get; set; }
        public Guid HotelId { get; set; }
        public ICollection<Reservation> ReservationsDate { get; set; }
        public ICollection<Guid> ServiceIds { get; set; }
        public List<RoomImageVM> RoomImageVMs { get; set; }
        public List<Guid> ImagesIds { get; set; }

    }

    public class RoomImageVM
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public bool IsPrimary { get; set; }
    }
}

