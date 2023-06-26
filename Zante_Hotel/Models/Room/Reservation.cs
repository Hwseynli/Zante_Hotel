namespace Zante_Hotel.Models
{
	public class Reservation:BaseEntity
	{
        [Required,MinLength(3),MaxLength(100)]
        public string FullName { get; set; }
        [Required, DataType(DataType.EmailAddress), StringLength(100)]
        public string Email { get; set; }
        [Required,DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime ArrivalDateTime { get; set; }
        [Required]
        public DateTime DepartureDateTime { get; set; }
        [Required]
        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
        [NotMapped]
        public string ArrivalDate => ArrivalDateTime.ToString("MM/dd/yyyy");
        [NotMapped]
        public string ArrivalTime => ArrivalDateTime.ToString("hh:mm tt");
        [NotMapped]
        public string DepartureDate => DepartureDateTime.ToString("MM/dd/yyyy");
        [NotMapped]
        public string DepartureTime => DepartureDateTime.ToString("hh:mm tt");
    }
}

