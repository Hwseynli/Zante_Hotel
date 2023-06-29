namespace Zante_Hotel.Models
{
	public class Gallery:BaseEntity
	{
		[Required,MinLength(5),MaxLength(200)]
		public string Url { get; set; }
	}
}

