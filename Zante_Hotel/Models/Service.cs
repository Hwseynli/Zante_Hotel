namespace Zante_Hotel.Models
{
	public class Service:BaseNameableEntity
	{
        public ICollection<Room> Rooms { get; set; }
    }
}

