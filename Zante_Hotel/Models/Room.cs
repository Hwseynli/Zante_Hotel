namespace Zante_Hotel.Models
{
	public class Room:BaseNameableEntity
	{
        public bool IsReservation { get; set; } = false;
        [Required]
        public decimal Price { get; set; }
        public ICollection<Service> Services { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public Guid ViewId { get; set; }
        [ForeignKey("ViewId")]
        public View View { get; set; }

    }
}

