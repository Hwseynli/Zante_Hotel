using System;
namespace Zante_Hotel.Models
{
	public class Setting:BaseEntity
	{
		[Required]
		public string Key { get; set; }
		[Required]
		public string Value { get; set; }
	}
}

