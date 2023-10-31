namespace Zante_Hotel.Areas.AppAdmin.Models
{
	public class Restaurant : BaseNameableEntity
    {
        [Required]
        public string SubTitle { get; set; }
        [Required, MinLength(3), MaxLength(2000)]
        public string Description { get; set; }
        public ICollection<RestaurantFood> RestFoods { get; set; }
        public ICollection<RestaurantImage> Images { get; set; }
        public int MaxPeople { get; set; }
        [Required]
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
    }
    public class RestaurantImage : BaseEntity
    {
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public bool? IsPrimary { get; set; }
        public Guid RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restourant { get; set; }
    }
}