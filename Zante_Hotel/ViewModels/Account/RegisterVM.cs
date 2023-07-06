namespace Zante_Hotel.ViewModels
{
	public class RegisterVM
	{
        public string Name { get; set; }
        
        public string Surname { get; set; } = "XXX";
     
        public string Gender { get; set; }
        [Required]
        public byte Age { get; set; }
        
        public string Username { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public IFormFile UserPhoto { get; set; }
    }
}

