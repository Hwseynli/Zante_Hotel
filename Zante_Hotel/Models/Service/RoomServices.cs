namespace Zante_Hotel.Models
{
	public class RoomServices:BaseEntity
	{
		[Required]
		public Guid RoomId { get; set; }
		[ForeignKey("RoomId")]
		public Room Room { get; set; }
		[Required]
		public Guid ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
	}
}

