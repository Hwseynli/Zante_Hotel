namespace Zante_Hotel.ViewModels
{
	public class CreateRestaurantVM
	{
        [Required]
        public string SubTitle { get; set; }
        [Required, MinLength(3), MaxLength(2000)]
        public string Description { get; set; }
        public ICollection<Guid> FoodIds { get; set; }
        public ICollection<RestaurantImage> Images { get; set; }
        public int MaxPeople { get; set; }
        [Required]
        public Guid HotelId { get; set; }
    }
}

