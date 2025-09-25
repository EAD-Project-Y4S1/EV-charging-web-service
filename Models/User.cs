/**
 * EVChargingWebService - User Model
 * Represents a system user (Backoffice or Station Operator).
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EVChargingWebService.Models
{
    /// <summary>
    /// Roles for application users.
    /// </summary>
    public enum UserRole
    {
        Backoffice = 1,
        StationOperator = 2
    }

    /// <summary>
    /// User entity stored in MongoDB.
    /// </summary>
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("role")]
        public UserRole Role { get; set; }

        [Required]
        [BsonElement("fullName")]
        public string FullName { get; set; } = string.Empty;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }
}


