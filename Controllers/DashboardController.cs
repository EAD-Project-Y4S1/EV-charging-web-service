/**
 * EVChargingWebService - DashboardController
 * Aggregates summary counts for the dashboard.
 */

using EVChargingWebService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVChargingWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEVOwnerService _ownerService;
        private readonly IChargingStationService _stationService;
        private readonly BookingService _bookingService;

        public DashboardController(
            IUserService userService,
            IEVOwnerService ownerService,
            IChargingStationService stationService,
            BookingService bookingService)
        {
            _userService = userService;
            _ownerService = ownerService;
            _stationService = stationService;
            _bookingService = bookingService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<object>> GetSummary()
        {
            try
            {
                // Aggregate counts. Repositories/services lack count methods, so fetch and count.
                var users = await _userService.GetAllAsync();
                var owners = await _ownerService.GetAllAsync();
                var stations = await _stationService.GetAllAsync();
                var summary = new
                {
                    users = users.Count,
                    owners = owners.Count,
                    stations = stations.Count,
                    bookings = 0
                };
                return Ok(summary);
            }
            catch (Exception ex)
            {
                // Hide internal error details from clients; log if needed.
                return StatusCode(500, new { message = "Failed to load dashboard summary" });
            }
        }
    }
}


