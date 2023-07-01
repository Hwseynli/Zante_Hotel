namespace Zante_Hotel.ViewModels
{
	public class CreateServiceVM
	{
        [Required]
        [MinLength(3)]
        [MaxLength(55)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Icon { get; set; }
    }
}