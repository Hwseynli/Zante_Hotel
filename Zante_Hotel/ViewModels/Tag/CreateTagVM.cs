namespace Zante_Hotel.ViewModels
{
	public class CreateTagVM
	{
		[Required, MinLength(3), MaxLength(100)]
		public string Name { get; set; }
	}
}

