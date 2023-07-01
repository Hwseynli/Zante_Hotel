using System;
namespace Zante_Hotel.ViewModels
{
	public class UpdateServiceVM
	{
		[StringLength(55)]
		public string Name { get; set; }
		[StringLength(200)]
		public string Icon { get; set; }
	}
}

