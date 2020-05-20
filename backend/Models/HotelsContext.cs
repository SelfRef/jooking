using Microsoft.EntityFrameworkCore;

namespace JookingApi.Models {
	public class HotelsContext: DbContext {
		public HotelsContext(DbContextOptions<HotelsContext> options): base(options) {

		}
		public DbSet<Hotel> Hotels { get; set; }
	}
}