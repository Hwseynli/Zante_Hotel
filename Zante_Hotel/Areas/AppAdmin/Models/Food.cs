using System;
namespace Zante_Hotel.Areas.AppAdmin.Models
{
	public class Food : BaseNameableEntity
    {
        [Required, StringLength(500)]
        public string Description { get; set; }
        [Required, StringLength(200)]
        public string ImgUrl { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}

