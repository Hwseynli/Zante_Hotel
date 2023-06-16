namespace Zante_Hotel.Models
{
	public class Room:BaseNameableEntity
	{
        public ICollection<Service> Services { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ViewId { get; set; }
        public View View { get; set; }

    }
}

