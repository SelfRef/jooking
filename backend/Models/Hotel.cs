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
	public static HotelResponse FromHotel(Hotel hotel) {
		return new HotelResponse {
			Id = hotel.Id,
			Name = hotel.Name,
			Description = hotel.Description,
			Email = hotel.Email,
			Phone = hotel.Phone,
			UserId = hotel.UserId,
			Rooms = hotel.Rooms
		};
	}
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public int UserId { get; set; }
	public ICollection<Room> Rooms { get; set; }
}

public class HotelRequest {
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public int UserId { get; set; }
}