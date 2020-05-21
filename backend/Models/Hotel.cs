using System.Collections.Generic;

public class Hotel {
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public User User { get; set; }
	public ICollection<Room> Rooms { get; set; }
}