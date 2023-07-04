namespace Zante_Hotel.Areas.AppAdmin.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<RoomServices> RoomServices { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Spa> Spas { get; set; }
        public DbSet<SpaImage> SpaImages { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantFood> RestaurantFoods { get; set; }
        public DbSet<RestaurantImage> RestaurantImages { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelService> HotelServices { get; set; }
        public DbSet<HotelComment> HotelComments { get; set; }
    }
}

