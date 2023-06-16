namespace Zante_Hotel.Models
{
	public class Category:BaseNameableEntity
	{
        public ICollection<Room> Rooms { get; set; }
    }
}

