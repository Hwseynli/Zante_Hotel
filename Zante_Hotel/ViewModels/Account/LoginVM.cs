namespace Zante_Hotel.ViewModels
{
	public class LoginVM
	{
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
