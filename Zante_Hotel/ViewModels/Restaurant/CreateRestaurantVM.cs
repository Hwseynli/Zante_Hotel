﻿using System;
namespace Zante_Hotel.ViewModels
{
	public class CreateRestaurantVM
	{
        [Required, MinLength(3), MaxLength(45)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string SubTitle { get; set; }
        [Required, MinLength(3), MaxLength(2000)]
        public string Description { get; set; }
        public ICollection<RestaurantFood> RestFoods { get; set; }
        public IFormFile MainPhoto { get; set; }
        public ICollection<IFormFile> Photos { get; set; }
        public ICollection<RestaurantImage> RestImages { get; set; }
        public ICollection<Guid> ImageIds { get; set; }
        public ICollection<Guid> FoodIds { get; set; }
        public int MaxPeople { get; set; }
        [Required]
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
    }
}

