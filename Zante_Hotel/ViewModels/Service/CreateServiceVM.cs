using System;
namespace Zante_Hotel.ViewModels
{
	public class CreateServiceVM
	{
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}

