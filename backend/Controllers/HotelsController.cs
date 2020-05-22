using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelixApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelixApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelsContext _context;

        public HotelsController(HotelsContext context)
        {
            _context = context;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelResponse>>> GetHotels()
        {
            return await _context.Hotels.Select(hotel => HotelResponse.FromHotel(hotel)).ToListAsync();
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelResponse>> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            await _context.Rooms.Where(r => r.HotelId == id).LoadAsync();

            return HotelResponse.FromHotel(hotel);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelRequest hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest();
            }

            var hotelModel = await _context.Hotels.FindAsync(id);
            hotelModel.Name = hotel.Name;
            hotelModel.Description = hotel.Description;
            hotelModel.Email = hotel.Email;
            hotelModel.Phone = hotel.Phone;
            hotelModel.UserId = hotel.UserId;
            _context.Entry(hotelModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HotelResponse>> PostHotel(HotelRequest hotel)
        {
            var hotelModel = new Hotel() {
                Id = 0,
                Name = hotel.Name,
                Description = hotel.Description,
                Email = hotel.Email,
                Phone = hotel.Phone,
                UserId = hotel.UserId
            };
            _context.Hotels.Add(hotelModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHotel", new { id = hotelModel.Id }, HotelResponse.FromHotel(hotelModel));
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelResponse>> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return HotelResponse.FromHotel(hotel);
        }

        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.Id == id);
        }
    }
}
