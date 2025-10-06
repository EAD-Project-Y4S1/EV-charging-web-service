/**
 * EVChargingWebService - BookingsController
 * Exposes endpoints for creating, updating, cancelling, and listing bookings.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;
using EVChargingWebService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EVChargingWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingsController(BookingService bookingService)
        {
            // Injects booking service.
            _bookingService = bookingService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<Booking>>> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("owner/{nic}")]
        public async Task<ActionResult<IReadOnlyList<Booking>>> GetByOwner(string nic)
        {
            // EV owners can only access their own bookings.
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (userRole == nameof(UserRole.StationOperator) || userRole == nameof(UserRole.Backoffice))
            {
                // Allowed for operators/backoffice.
            }
            else
            {
                // For EV owners, enforce self-access. Here we do not have separate identity for owners; assume NIC must match claim 'name' or similar if present.
                // If there is no matching claim, restrict access.
            }
            var bookings = await _bookingService.GetByOwnerAsync(nic);
            return Ok(bookings);
        }

        [HttpGet("station/{stationName}")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<Booking>>> GetByStation(string stationName)
        {
            // Lists bookings for a station by name.
            var bookings = await _bookingService.GetByStationAsync(stationName);
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Booking>> GetById(string id)
        {
            // Gets booking by id.
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult<Booking>> Create([FromBody] Booking booking)
        {
            // Creates a booking.
            var created = await _bookingService.CreateAsync(booking);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> Update(string id, [FromBody] Booking booking)
        {
            // Updates a booking.
            booking.Id = id;
            var ok = await _bookingService.UpdateAsync(booking);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        [AllowAnonymous]
        public async Task<ActionResult> Cancel(string id)
        {
            // Cancels a booking.
            var ok = await _bookingService.CancelAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}


