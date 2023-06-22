namespace Zante_Hotel.Models.Base
{
	public abstract class BaseNameableEntity:BaseEntity
	{
		[Required,MinLength(3),MaxLength(25)]
		public string Name { get; set; }
	}
}

