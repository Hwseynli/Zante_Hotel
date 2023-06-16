using System;
namespace Zante_Hotel.Models.Base
{
	public abstract class BaseNameableEntity:BaseEntity
	{
		public string Name { get; set; }
	}
}

