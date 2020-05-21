using System;
using System.Collections.Generic;

public class User {
	public int Id { get; set; }
	public string Name { get; set; }
	public string Surname { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public DateTime Registered { get; set; }
	public Role Role { get; set; }
	public ICollection<Reservation> Reservations { get; set; }
	public ICollection<Hotel> Hotels { get; set; }
}

public enum Role {
	Guest,
	Moderator,
	Admin
}