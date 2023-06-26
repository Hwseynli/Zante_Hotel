namespace Zante_Hotel.Models
{
	public class Service:BaseNameableEntity
	{
        public string Icon { get; set; }
        public ICollection<RoomServices> Rooms { get; set; }
    }
}

