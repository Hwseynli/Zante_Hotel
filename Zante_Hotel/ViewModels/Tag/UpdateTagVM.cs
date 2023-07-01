using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateTagVM
	{
		[StringLength(100)]
		public string Name { get; set; }
	}
}

