/**
 * EVChargingWebService - EVOwner Model
 * Represents an EV owner identified by NIC.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace EVChargingWebService.Models
{
    /// <summary>
    /// Account status for an EV owner.
    /// </summary>
    public enum OwnerStatus
    {
        Inactive = 0,
        Active = 1
    }

    /// <summary>
    /// EVOwner entity stored in MongoDB.
    /// </summary>
    public class EVOwner
    {
        [Key]
        [Required]
        [BsonId]
        [BsonElement("nic")]
        public string NIC { get; set; } = string.Empty;

        [Required]
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [BsonElement("email")]
        public string? Email { get; set; }

        [Phone]
        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("vehicleDetails")]
        public string? VehicleDetails { get; set; }

        [BsonElement("status")]
        public OwnerStatus Status { get; set; } = OwnerStatus.Active;
    }
}


