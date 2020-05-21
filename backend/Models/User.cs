using System;

public class User {
	public int Id { get; set; }
	public string Name { get; set; }
	public string Surname { get; set; }
	public string Email { get; set; }
	public DateTime Registered { get; set; }
	public Role Role { get; set; }
	public Reservation[] Reservations { get; set; }
	public Hotel[] Hotels { get; set; }
}

public enum Role {
	Guest,
	Moderator,
	Admin
}