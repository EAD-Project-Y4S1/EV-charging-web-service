/**
 * EVChargingWebService - AuthController
 * Handles user authentication and issues JWT tokens.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EVChargingWebService.Config;
using EVChargingWebService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EVChargingWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IUserService userService, IOptions<JwtSettings> jwtOptions)
        {
            // Injects dependencies.
            _userService = userService;
            _jwtSettings = jwtOptions.Value;
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
        {
            // Validates credentials and returns JWT.
            var user = await _userService.ValidateCredentialsAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryInHours);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { access_token = tokenString, expires_at = expires });
        }
    }
}


