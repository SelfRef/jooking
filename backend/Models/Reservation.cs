using System;

public class Reservation {
	public int Id { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public Room Room { get; set; }
	public User User { get; set; }
}