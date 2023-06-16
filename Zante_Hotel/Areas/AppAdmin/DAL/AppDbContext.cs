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
    }
}

