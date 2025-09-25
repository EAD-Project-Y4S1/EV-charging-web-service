/**
 * EVChargingWebService - EVOwnersController
 * Exposes CRUD and activation endpoints for EV owners.
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
    public class EVOwnersController : ControllerBase
    {
        private readonly IEVOwnerService _ownerService;

        public EVOwnersController(IEVOwnerService ownerService)
        {
            // Injects owner service.
            _ownerService = ownerService;
        }

        [HttpGet]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult<IReadOnlyList<EVOwner>>> GetAll()
        {
            // Lists all EV owners.
            var owners = await _ownerService.GetAllAsync();
            return Ok(owners);
        }

        [HttpGet("{nic}")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult<EVOwner>> GetByNIC(string nic)
        {
            // Gets owner by NIC.
            var owner = await _ownerService.GetByNICAsync(nic);
            if (owner == null) return NotFound();
            return Ok(owner);
        }

        [HttpPost]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult<EVOwner>> Create([FromBody] EVOwner owner)
        {
            // Creates a new owner.
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _ownerService.CreateAsync(owner);
            return CreatedAtAction(nameof(GetByNIC), new { nic = created.NIC }, created);
        }

        [HttpPut("{nic}")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> Update(string nic, [FromBody] EVOwner owner)
        {
            // Updates an owner.
            owner.NIC = nic;
            var ok = await _ownerService.UpdateAsync(owner);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{nic}")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> Delete(string nic)
        {
            // Deletes an owner.
            var ok = await _ownerService.DeleteAsync(nic);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPost("{nic}/activate")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> Activate(string nic)
        {
            // Activates owner account.
            var ok = await _ownerService.ActivateAsync(nic);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPost("{nic}/deactivate")]
        [Authorize(Roles = "Backoffice,StationOperator")]
        public async Task<ActionResult> Deactivate(string nic)
        {
            // Deactivates owner account.
            var ok = await _ownerService.DeactivateAsync(nic);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}


