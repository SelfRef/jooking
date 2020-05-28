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
					Phone = "123456789",
					Password = "qwerty",
					Name = "Jan",
					Surname = "Kowalski",
					Registered = DateTime.Now
				},
				new User() {
					Id = 2,
					Role = Role.Moderator,
					Email = "nowak@mail.com",
					Phone = "123456789",
					Password = "qwerty",
					Name = "Jan",
					Surname = "Nowak",
					Registered = DateTime.Now
				},
				new User() {
					Id = 3,
					Role = Role.Guest,
					Email = "nosacz@mail.com",
					Phone = "123456789",
					Password = "qwerty",
					Name = "Janusz",
					Surname = "Nosacz",
					Registered = DateTime.Now
				}
			};

			var hotels = new Hotel[] {
				new Hotel() {
					Id = 1,
					Email = "hotel1@mail.com",
					Phone = "123456789",
					Description = "qwerty",
					Name = "Hotel 1",
					UserId = 2
				},
				new Hotel() {
					Id = 2,
					Email = "hotel2@mail.com",
					Description = "qwerty",
					Phone = "123456789",
					Name = "Hotel 2",
					UserId = 2
				},
				new Hotel() {
					Id = 3,
					Email = "hotel3@mail.com",
					Phone = "123456789",
					Description = "qwerty",
					Name = "Hotel 3",
					UserId = 2
				}
			};

			var rooms = new Room[] {
				new Room() {
					Id = 1,
					Number = 100,
					Standard = RoomStandard.Onion,
					BedCount = 2,
					BedSize = 1,
					HotelId = 1,
				}
			};

			var reservations = new Reservation[] {
				new Reservation() {
					Id = 1,
					StartDate = DateTime.Parse("2020-01-01"),
					EndDate = DateTime.Parse("2020-01-15"),
					RoomId = 1,
					UserId = 3,
					Phone = "123456789",
					Email = "abcd"
				},
				new Reservation() {
					Id = 2,
					StartDate = DateTime.Parse("2020-02-01"),
					EndDate = DateTime.Parse("2020-02-15"),
					RoomId = 1,
					UserId = 3,
					Phone = "123456789",
					Email = "abcd"
				},
				new Reservation() {
					Id = 3,
					StartDate = DateTime.Parse("2020-03-01"),
					EndDate = DateTime.Parse("2020-03-15"),
					RoomId = 1,
					UserId = 3,
					Phone = "123456789",
					Email = "abcd"
				}
			};

			modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
			modelBuilder.Entity<User>().HasMany(u => u.Hotels).WithOne(h => h.User).HasForeignKey(h => h.UserId);
			modelBuilder.Entity<User>().HasMany(u => u.Reservations).WithOne(r => r.User).HasForeignKey(r => r.UserId);
			modelBuilder.Entity<Room>().HasMany(u => u.Reservations).WithOne(r => r.Room).HasForeignKey(r => r.RoomId);
			modelBuilder.Entity<Hotel>().HasMany(h => h.Rooms).WithOne(r => r.Hotel).HasForeignKey(r => r.HotelId);
			
			modelBuilder.Entity<User>().HasData(users);
			modelBuilder.Entity<Hotel>().HasData(hotels);
			modelBuilder.Entity<Room>().HasData(rooms);
			modelBuilder.Entity<Reservation>().HasData(reservations);
		}
	}
}