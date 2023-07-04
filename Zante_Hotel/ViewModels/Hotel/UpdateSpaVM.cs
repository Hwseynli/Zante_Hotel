using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateSpaVM
	{
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(200)]
        public string SubTitle { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(20000)]
        public string Decription { get; set; }
        public Guid HotelId { get; set; }
        public ICollection<SpaImage> Images { get; set; }
        public ICollection<SpaImageVM> ImageVMs { get; set; }
        public IFormFile MainPhoto { get; set; }
        public ICollection<IFormFile> Photos { get; set; }
        public List<Guid> ImagesIds { get; set; }

    }
    public class SpaImageVM
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public bool IsPrimary { get; set; }
    }
}

