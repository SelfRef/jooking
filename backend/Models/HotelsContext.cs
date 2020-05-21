using Microsoft.EntityFrameworkCore;

namespace JookingApi.Models {
	public class HotelsContext: DbContext {
		public HotelsContext(DbContextOptions<HotelsContext> options): base(options) {}
		public DbSet<Hotel> Hotels { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<User> Users { get; set; }
	}
}