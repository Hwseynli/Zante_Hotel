namespace Zante_Hotel.Models
{
	public class RoomServices:BaseEntity
	{
		public Guid RoomId { get; set; }
		public Room Room { get; set; }
		public Guid ServiceId { get; set; }
		public Service Service { get; set; }
	}
}

