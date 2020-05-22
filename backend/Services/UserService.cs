using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using HotelixApi.Models;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System;

namespace HotelixApi.Services
{
	public interface IUserService
	{
		User Authenticate(string username, string password);
	}

	public class UserService : IUserService
	{
		// users hardcoded for simplicity, store in a db with hashed passwords in production applications
		private List<User> _users = new List<User>
				{
						new User { Id = 1, Name = "Test", Surname = "User", Email = "test", Password = "test" }
				};

		private readonly IConfiguration _config;
		private readonly HotelsContext _context;

		public UserService(IConfiguration config, HotelsContext context)
		{
			_config = config;
			_context = context;
		}

		public User Authenticate(string email, string password)
		{
			var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

			if (user == null)
				return null;

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_config["JwtToken:SecretKey"]);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString()),
					new Claim(ClaimTypes.Role, user.Role)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);

			return user;
		}
	}
}