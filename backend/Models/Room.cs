using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

public class Room {
	public int Id { get; set; }
	public int Number { get; set; }
	public RoomStandard Standard { get; set; }
	public int BedCount { get; set; }
	public int BedSize { get; set; }
	[NotMapped]
	public Hotel Hotel { get; set; }
	public int HotelId { get; set; }
	public ICollection<Reservation> Reservations { get; set; }
}

public enum RoomStandard {
	Onion,
	Comfort,
	Luxury
}