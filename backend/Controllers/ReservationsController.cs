using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JookingApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JookingApi.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationsController : ControllerBase
	{
		private readonly HotelsContext _context;
		private readonly IHttpContextAccessor _httpContext;

		public ReservationsController(HotelsContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContext = httpContextAccessor;
		}

		// GET: api/Reservations
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
		{
			return await _context.Reservations.ToListAsync();
		}

		// GET: api/Reservations/5
		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<Reservation>> GetReservation(int id)
		{
			var reservation = await _context.Reservations.FindAsync(id);

			if (reservation == null)
			{
				return NotFound();
			}

			return reservation;
		}
		// GET: api/Reservations/User
		[HttpGet("User")]
		public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationUser()
		{
			var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

			if (userId == null)
			{
				return NotFound();
			}

			return await _context.Reservations
				.Where(r => r.UserId.ToString() == userId)
				.Include(r => r.Room)
					.ThenInclude(r => r.Hotel)
				.ToListAsync();
		}

		// PUT: api/Reservations/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> PutReservation(int id, Reservation reservation)
		{
			if (id != reservation.Id)
			{
				return BadRequest();
			}

			_context.Entry(reservation).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ReservationExists(id))
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

		// POST: api/Reservations
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
		{
			var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
			if (userId == null)
			{
				return NotFound();
			}

			reservation.UserId = int.Parse(userId);
			_context.Reservations.Add(reservation);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
		}

		// DELETE: api/Reservations/5
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<Reservation>> DeleteReservation(int id)
		{
			var reservation = await _context.Reservations.FindAsync(id);
			if (reservation == null)
			{
				return NotFound();
			}

			_context.Reservations.Remove(reservation);
			await _context.SaveChangesAsync();

			return reservation;
		}

		private bool ReservationExists(int id)
		{
			return _context.Reservations.Any(e => e.Id == id);
		}
	}
}
