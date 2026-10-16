using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs.Enrolments;
using RaceDay.API.Models;
using System.Security.Claims;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/enrolments")]
    [Authorize]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Enrol in an event category (Participant only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> CreateEnrolment([FromBody] CreateEnrolmentDto dto)
        {
            var participantId = GetCurrentUserId();

            var eventCategory = await _context.EventCategories
                .Include(ec => ec.Event)
                .FirstOrDefaultAsync(ec => ec.Event_Category_ID == dto.Event_Category_ID);

            if (eventCategory == null)
                return NotFound(new { message = "Event category not found" });

            // Check if already enrolled
            var existing = await _context.Enrolments
                .AnyAsync(e => e.Event_Category_ID == dto.Event_Category_ID
                            && e.Participant_ID == participantId);

            if (existing)
                return BadRequest(new { message = "Already enrolled in this category" });

            // Check capacity
            var enrolmentCount = await _context.Enrolments
                .CountAsync(e => e.Event_Category_ID == dto.Event_Category_ID
                              && e.Status != "Cancelled");

            if (enrolmentCount >= eventCategory.Event!.Max_Capacity)
                return BadRequest(new { message = "Event capacity full" });

            var enrolment = new Enrolment
            {
                Event_Category_ID = dto.Event_Category_ID,
                Participant_ID = participantId,
                Enrolment_Date = DateTime.Now,
                Status = "Pending"
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                enrolment.Enrolment_ID,
                enrolment.Event_Category_ID,
                enrolment.Participant_ID,
                enrolment.Enrolment_Date,
                enrolment.Status
            });
        }

        /// <summary>
        /// Get current participant's enrolments
        /// </summary>
        [HttpGet("my")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var participantId = GetCurrentUserId();

            var enrolments = await _context.Enrolments
                .Where(e => e.Participant_ID == participantId)
                .Include(e => e.EventCategory)
                    .ThenInclude(ec => ec!.Event)
                .Include(e => e.EventCategory)
                    .ThenInclude(ec => ec!.Category)
                .Include(e => e.Result)
                .Select(e => new
                {
                    e.Enrolment_ID,
                    Event_ID = e.EventCategory!.Event_ID,
                    Event_Name = e.EventCategory.Event!.Event_Name,
                    Category_Name = e.EventCategory.Category!.Category_Name,
                    Entry_Fee = e.EventCategory.Entry_Fee,
                    e.Enrolment_Date,
                    e.Status,
                    Result = e.Result == null ? null : new
                    {
                        e.Result.Finish_Time,
                        e.Result.Position,
                        e.Result.Status
                    }
                })
                .ToListAsync();

            return Ok(new { enrolments });
        }

        /// <summary>
        /// Get all enrolments for an event (Organiser only)
        /// </summary>
        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEventEnrolments(int eventId)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null)
                return NotFound(new { message = "Event not found" });

            if (ev.Organiser_ID != GetCurrentUserId())
                return Forbid();

            var enrolments = await _context.Enrolments
                .Where(e => e.EventCategory!.Event_ID == eventId)
                .Include(e => e.Participant)
                .Include(e => e.EventCategory)
                    .ThenInclude(ec => ec!.Category)
                .Select(e => new
                {
                    e.Enrolment_ID,
                    Participant_ID = e.Participant_ID,
                    Participant_Name = e.Participant!.Full_Name,
                    Category_Name = e.EventCategory!.Category!.Category_Name,
                    e.Enrolment_Date,
                    e.Status
                })
                .ToListAsync();

            return Ok(new
            {
                Event_ID = eventId,
                Event_Name = ev.Event_Name,
                Enrolments = enrolments
            });
        }

        /// <summary>
        /// Update enrolment status (Organiser only)
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateEnrolmentStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            var enrolment = await _context.Enrolments
                .Include(e => e.EventCategory)
                    .ThenInclude(ec => ec!.Event)
                .FirstOrDefaultAsync(e => e.Enrolment_ID == id);

            if (enrolment == null)
                return NotFound(new { message = "Enrolment not found" });

            if (enrolment.EventCategory!.Event!.Organiser_ID != GetCurrentUserId())
                return Forbid();

            var validStatuses = new[] { "Pending", "Approved", "Rejected", "Cancelled", "Completed" };
            if (!validStatuses.Contains(dto.Status))
                return BadRequest(new { message = "Invalid status value" });

            enrolment.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                enrolment.Enrolment_ID,
                enrolment.Status,
                Updated_At = DateTime.Now
            });
        }
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}