namespace Zante_Hotel.ViewModels
{
	public class HomeVM
	{
		public Hotel Hotel { get; set; }
		public ICollection<Room> Rooms { get; set; }
		public Room Room { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
        public Slider Slider { get; set; }
		public ICollection<Blog> Blogs { get; set; }
	}
}

