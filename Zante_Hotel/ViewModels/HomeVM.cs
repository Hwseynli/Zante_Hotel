﻿namespace Zante_Hotel.ViewModels
{
	public class HomeVM
	{
		public ICollection<Room> Rooms { get; set; }
		public Room Room { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
        public Slider Slider { get; set; }
    }
}

