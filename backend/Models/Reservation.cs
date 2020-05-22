using System;

public class Reservation {
	public int Id { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public Room Room { get; set; }
	public int RoomId { get; set; }
	public User User { get; set; }
	public int UserId { get; set; }
}