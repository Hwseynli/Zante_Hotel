using System;
namespace Zante_Hotel.Areas.AppAdmin.Models
{
	public class Food : BaseNameableEntity
    {
        [Required, StringLength(500)]
        public string About { get; set; }
        [Required, StringLength(200)]
        public string ImageUrl { get; set; }
        [Required]
        public decimal Price { get; set; }
        public ICollection<RestaurantFood> RestFoods { get; set; }
    }
}

