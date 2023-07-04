using System;
namespace Zante_Hotel.ViewModels
{
	public class CreateSettingVM
	{
		[Required]
		public string Key { get; set; }
		[Required]
		public string Value { get; set; }
	}
}

