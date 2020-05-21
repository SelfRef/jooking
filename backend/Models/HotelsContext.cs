using System;
using Microsoft.EntityFrameworkCore;

namespace JookingApi.Models {
	public class HotelsContext: DbContext {
		public DbSet<Hotel> Hotels { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<User> Users { get; set; }
		public HotelsContext(DbContextOptions<HotelsContext> options): base(options) {}
		public HotelsContext(): base() {}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseInMemoryDatabase("Hotels");
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			var users = new User[] {
				new User() {
					Id = 1,
					Role = Role.Admin,
					Email = "kowalski@mail.com",
					Password = "qwerty",
					Name = "Jan",
					Surname = "Kowalski",
					Registered = DateTime.Now
				},
				new User() {
					Id = 2,
					Role = Role.Moderator,
					Email = "nowak@mail.com",
					Password = "qwerty",
					Name = "Jan",
					Surname = "Nowak",
					Registered = DateTime.Now
				},
				new User() {
					Id = 3,
					Role = Role.Guest,
					Email = "nosacz@mail.com",
					Password = "qwerty",
					Name = "Janusz",
					Surname = "Nosacz",
					Registered = DateTime.Now
				}
			};

			modelBuilder.Entity<User>().HasData(users);
		}
	}
}