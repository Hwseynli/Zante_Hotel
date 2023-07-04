using System;
using System.Globalization;

namespace Zante_Hotel.ViewModels
{
	public class ReservationVM
	{
        [Required, MinLength(3), MaxLength(35)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Surname { get; set; }
        [Required, DataType(DataType.EmailAddress), StringLength(100)]
        public string Email { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        public string Comment { get; set; }
        [Required]
        public DateTime ArrivalDateTime { get; set; }
        [Required]
        public DateTime DepartureDateTime { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
        //[Required]
        //public DateOnly ArrivalDate { get; set; }
        //[Required]
        //public TimeSpan ArrivalTime { get; set; }
        //[Required]
        //public DateOnly DepartureDate { get; set; }
        //[Required]
        //public TimeSpan DepartureTime { get; set; }

    }
}

