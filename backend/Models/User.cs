using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class User {
	public int Id { get; set; }
	public string Name { get; set; }
	public string Surname { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	[JsonIgnore]
	public string Password { get; set; }
	public string Token { get; set; }
	public DateTime Registered { get; set; }
	public string Role { get; set; }
	public ICollection<Reservation> Reservations { get; set; }
	public ICollection<Hotel> Hotels { get; set; }
}

public static class Role {
	public const string Guest = "Guest";
	public const string Moderator = "Moderator";
	public const string Admin = "Admin";
}

public class UserResponse {
	public static UserResponse FromUser(User user) {
		return new UserResponse {
			Id = user.Id,
			Name = user.Name,
			Surname = user.Surname,
			Email = user.Email,
			Phone = user.Phone,
			Registered = user.Registered,
			Role = user.Role,
			Reservations = user.Reservations,
		};
	}
	public int Id { get; set; }
	public string Name { get; set; }
	public string Surname { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public DateTime Registered { get; set; }
	public string Role { get; set; }
	public ICollection<Reservation> Reservations { get; set; }
}

public class UserRequest {
	public int Id { get; set; }
	public string Name { get; set; }
	public string Surname { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public string Role { get; set; }
}