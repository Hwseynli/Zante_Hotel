namespace Zante_Hotel.Models.Base
{
	public abstract class BaseNameableEntity:BaseEntity
	{
		[Required,MinLength(3),MaxLength(55)]
		public string Name { get; set; }
	}
}

