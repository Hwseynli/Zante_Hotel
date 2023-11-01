namespace Zante_Hotel.Models
{
	public class Message:BaseEntity
	{
        [Required,MinLength(3),MaxLength(25)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(45)]
        public string Surname { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }
        [Required,StringLength(1000)]
        public string Body { get; set; }
        
        public DateTime CreateOn { get; set; }
    }
}