namespace Zante_Hotel.ViewModels
{
	public class CreateFoodVM
	{
        [Required,MinLength(3),MaxLength(50)]
        public string Name { get; set; }
        [Required, StringLength(500)]
        public string About { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
