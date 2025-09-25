/**
 * EVChargingWebService - Booking Model
 * Represents a booking for a charging station by an EV owner.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EVChargingWebService.Models
{
    /// <summary>
    /// Booking status lifecycle.
    /// </summary>
    public enum BookingStatus
    {
        Active = 1,
        Cancelled = 2,
        Completed = 3
    }

    /// <summary>
    /// Booking entity stored in MongoDB.
    /// </summary>
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [BsonElement("ownerNIC")]
        public string OwnerNIC { get; set; } = string.Empty;

        [Required]
        [BsonElement("stationId")]
        public string StationId { get; set; } = string.Empty;

        [Required]
        [BsonElement("reservationDateTime")]
        public DateTime ReservationDateTime { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("status")]
        public BookingStatus Status { get; set; } = BookingStatus.Active;
    }
}


