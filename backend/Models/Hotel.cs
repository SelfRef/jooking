using System.Collections.Generic;

public class Hotel {
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public User User { get; set; }
	public int UserId { get; set; }
	public ICollection<Room> Rooms { get; set; }
}

public class HotelResponse {
	public static HotelResponse FromHotel(Hotel user) {
		return new HotelResponse {
			Id = user.Id,
			Name = user.Name,
			Description = user.Description,
			Email = user.Email,
			Phone = user.Phone,
			UserId = user.UserId
		};
	}
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public int UserId { get; set; }
}

public class HotelRequest {
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public int UserId { get; set; }
}