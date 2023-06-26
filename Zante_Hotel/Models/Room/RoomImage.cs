namespace Zante_Hotel.Models
{
	public class RoomImage:BaseEntity
	{
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public bool IsPrimary { get; set; }
        [Required]
        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
    }
}

