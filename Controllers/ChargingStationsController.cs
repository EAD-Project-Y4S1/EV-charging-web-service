/**
 * EVChargingWebService - ChargingStationsController
 * Exposes CRUD and activation endpoints for charging stations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;
using EVChargingWebService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVChargingWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChargingStationsController : ControllerBase
    {
        private readonly IChargingStationService _stationService;

        public ChargingStationsController(IChargingStationService stationService)
        {
            // Injects station service.
            _stationService = stationService;
        }

        [HttpGet]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult<IReadOnlyList<ChargingStation>>> GetAll()
        {
            // Lists all stations.
            var stations = await _stationService.GetAllAsync();
            return Ok(stations);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult<ChargingStation>> GetById(string id)
        {
            // Gets a station by id.
            var station = await _stationService.GetByIdAsync(id);
            if (station == null) return NotFound();
            return Ok(station);
        }

        [HttpPost]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult<ChargingStation>> Create([FromBody] ChargingStation station)
        {
            // Creates a new station.
            var created = await _stationService.CreateAsync(station);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> Update(string id, [FromBody] ChargingStation station)
        {
            // Updates a station.
            station.Id = id;
            var ok = await _stationService.UpdateAsync(station);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Backoffice")]
        public async Task<ActionResult> Delete(string id)
        {
            // Deletes a station.
            var ok = await _stationService.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/activate")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> Activate(string id)
        {
            // Activates station.
            var ok = await _stationService.ActivateAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> Deactivate(string id)
        {
            // Deactivates station if allowed.
            var ok = await _stationService.DeactivateAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

        public class UpdateScheduleRequest
        {
            public List<string> Schedule { get; set; } = new List<string>();
        }

        [HttpPost("{id}/schedule")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> UpdateSchedule(string id, [FromBody] UpdateScheduleRequest request)
        {
            // Updates station schedule.
            var ok = await _stationService.UpdateScheduleAsync(id, request.Schedule);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}


