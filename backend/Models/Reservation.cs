using System;

public class Reservation {
	public int Id { get; set; }
	public User Reserving { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}