using System;
namespace Zante_Hotel.ViewModels
{
	public class CreateSpaVM
	{
        [Required,MaxLength(40),MinLength(3)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string SubTitle { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string Title { get; set; }
        [Required, MinLength(3), MaxLength(20000)]
        public string Decription { get; set; }
        [Required]
        public Guid HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
        [Required]
        public IFormFile MainPhoto { get; set; }
        public ICollection<IFormFile> Photos { get; set; }
        public List<Guid> ImagesIds { get; set; }

    }
}

