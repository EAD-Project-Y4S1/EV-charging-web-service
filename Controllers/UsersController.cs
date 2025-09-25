/**
 * EVChargingWebService - UsersController
 * Exposes CRUD endpoints for managing users.
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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            // Injects user service.
            _userService = userService;
        }

        public class CreateUserRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public UserRole Role { get; set; }
        }

        public class UpdateUserRequest
        {
            public string Email { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public UserRole Role { get; set; }
            public bool IsActive { get; set; } = true;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<User>>> GetAll()
        {
            // Returns all users.
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetById(string id)
        {
            // Returns a user by id.
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest request)
        {
            // Creates a user.
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userService.CreateAsync(request.Email, request.Password, request.FullName, request.Role);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(string id, [FromBody] UpdateUserRequest request)
        {
            // Updates user fields.
            var updated = await _userService.UpdateAsync(id, request.Email, request.FullName, request.Role, request.IsActive);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            // Deletes a user by id.
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}


