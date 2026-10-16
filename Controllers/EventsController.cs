using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs.Events;
using RaceDay.API.Models;
using System.Security.Claims;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Get all active events (Public - Any role)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _context.Events
                .Where(e => e.Is_Active)
                .Include(e => e.Organiser)
                .Select(e => new
                {
                    e.Event_ID,
                    e.Event_Name,
                    e.Description,
                    e.Event_Date,
                    e.Location,
                    e.Max_Capacity,
                    e.Is_Active,
                    Organiser_Name = e.Organiser!.Full_Name
                })
                .ToListAsync();

            return Ok(new { events });
        }

        /// <summary>
        /// Get a specific event by ID (Public)
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventById(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Organiser)
                .Include(e => e.EventCategories)
                    .ThenInclude(ec => ec.Category)
                .FirstOrDefaultAsync(e => e.Event_ID == id);

            if (ev == null)
                return NotFound(new { message = "Event not found" });

            return Ok(new
            {
                ev.Event_ID,
                ev.Event_Name,
                ev.Description,
                ev.Event_Date,
                ev.Location,
                ev.Max_Capacity,
                ev.Is_Active,
                Organiser = new { ev.Organiser!.User_ID, ev.Organiser.Full_Name },
                Categories = ev.EventCategories.Select(ec => new
                {
                    ec.Category_ID,
                    ec.Category!.Category_Name,
                    ec.Entry_Fee,
                    ec.Start_Time
                })
            });
        }

        /// <summary>
        /// Create a new event (Organiser only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var ev = new Event
            {
                Organiser_ID = GetCurrentUserId(),
                Event_Name = dto.Event_Name,
                Description = dto.Description,
                Event_Date = dto.Event_Date,
                Location = dto.Location,
                Max_Capacity = dto.Max_Capacity,
                Is_Active = true,
                Created_At = DateTime.Now
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                ev.Event_ID,
                ev.Event_Name,
                ev.Description,
                ev.Event_Date,
                ev.Location,
                ev.Max_Capacity,
                ev.Is_Active,
                ev.Created_At
            });
        }

        /// <summary>
        /// Update an existing event (Organiser only - must own the event)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto dto)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound(new { message = "Event not found" });

            if (ev.Organiser_ID != GetCurrentUserId())
                return Forbid();

            ev.Event_Name = dto.Event_Name;
            ev.Description = dto.Description;
            ev.Event_Date = dto.Event_Date;
            ev.Location = dto.Location;
            ev.Max_Capacity = dto.Max_Capacity;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                ev.Event_ID,
                ev.Event_Name,
                ev.Description,
                ev.Event_Date,
                ev.Location,
                ev.Max_Capacity
            });
        }

        /// <summary>
        /// Soft delete an event (Organiser only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound(new { message = "Event not found" });

            if (ev.Organiser_ID != GetCurrentUserId())
                return Forbid();

            ev.Is_Active = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event deactivated successfully" });
        }
    }
}