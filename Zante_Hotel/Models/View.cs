namespace Zante_Hotel.Models
{
	public class View:BaseNameableEntity
	{
		public ICollection<Room> Rooms { get; set; }
	}
}

