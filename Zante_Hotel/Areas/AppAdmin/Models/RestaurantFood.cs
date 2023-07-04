using System;
namespace Zante_Hotel.Areas.AppAdmin.Models
{
	public class RestaurantFood:BaseEntity
	{
        [Required]
        public Guid FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food Food { get; set; }
        [Required]
        public Guid RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }
    }
}

