using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateViewVM
	{
        [StringLength(100)]
        public string Name { get; set; }
    }
}

