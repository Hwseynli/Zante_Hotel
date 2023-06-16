namespace Zante_Hotel.Areas.AppAdmin.Models
{
	public class AppUser:IdentityUser
	{
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        [MinLength(3)]
        [MaxLength(25)]
        public string Surname { get; set; } = "XXX";
        [Required]
        public string Gender { get; set; }
        [Required]
        public byte Age { get; set; }
        public string UserImgUrl { get; set; }
    }
}

