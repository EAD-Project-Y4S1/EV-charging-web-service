/**
 * EVChargingWebService - JwtSettings
 * Defines configuration settings for JWT authentication.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

namespace EVChargingWebService.Config
{
    /// <summary>
    /// JWT configuration settings bound from appsettings.
    /// </summary>
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryInHours { get; set; }
    }
}


