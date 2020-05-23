using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelixApi.Models;
using Microsoft.AspNetCore.Authorization;
using HotelixApi.Services;

namespace HotelixApi.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly HotelsContext _context;
		private readonly IUserService _userService;

		public UsersController(HotelsContext context, IUserService userService)
		{
			_context = context;
			_userService = userService;
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]
		public ActionResult<User> Authenticate(Login model)
		{
			var user = _userService.Authenticate(model.Email, model.Password);

			if (user == null)
				return BadRequest(new { message = "Username or password is incorrect" });

			return user;
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<ActionResult<UserResponse>> PostUser(UserRegisterRequest user)
		{
			var currentUser = await _context.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
			if (currentUser != null) {
				return BadRequest(new { message = "User with this email already exists" });
			}
			var userModel = new User()
			{
				Id = 0,
				Name = user.Name,
				Surname = user.Surname,
				Email = user.Email,
				Phone = user.Phone,
				Password = user.Password,
				Role = Role.Guest,
				Registered = DateTime.Now
			};
			_context.Users.Add(userModel);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = userModel.Id }, UserResponse.FromUser(userModel));
		}

		// GET: api/Users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
		{
			return await _context.Users.Select(user => UserResponse.FromUser(user)).ToListAsync();
		}

		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<ActionResult<UserResponse>> GetUser(int id)
		{
			var user = await _context.Users.FindAsync(id);
			await _context.Reservations
			.Include(r => r.Room)
				.ThenInclude(r => r.Hotel)
			.Where(r => r.UserId == id)
			.LoadAsync();

			if (user == null)
			{
				return NotFound();
			}

			return UserResponse.FromUser(user);
		}

		// PUT: api/Users/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser(int id, UserRequest user)
		{
			if (id != user.Id)
			{
				return BadRequest();
			}

			var userModel = await _context.Users.FindAsync(id);
			userModel.Name = user.Name;
			userModel.Surname = user.Surname;
			userModel.Email = user.Email;
			userModel.Phone = user.Phone;
			userModel.Role = user.Role;
			if (!string.IsNullOrEmpty(user.Password)) {
				userModel.Password = user.Password;
			}

			_context.Entry(userModel).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Users
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<UserResponse>> PostUser(UserRequest user)
		{
			var userModel = new User()
			{
				Id = 0,
				Name = user.Name,
				Surname = user.Surname,
				Email = user.Email,
				Phone = user.Phone,
				Password = user.Password,
				Role = user.Role,
				Registered = DateTime.Now
			};
			_context.Users.Add(userModel);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = userModel.Id }, UserResponse.FromUser(userModel));
		}


		// DELETE: api/Users/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<User>> DeleteUser(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return user;
		}

		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}
