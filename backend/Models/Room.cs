public class Room {
	public int Id { get; set; }
	public int Number { get; set; }
	public RoomStandard Standard { get; set; }
	public int BedCount { get; set; }
	public int BedSize { get; set; }
	public Reservation[] Reservations { get; set; }
}

public enum RoomStandard {
	Onion,
	Comfort,
	Luxyry
}