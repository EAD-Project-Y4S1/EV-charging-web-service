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
        Confirmed = 1,
        Active = 2,
        Cancelled = 3,
        Completed = 4
    }

    /// <summary>
    /// Booking entity stored in MongoDB.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [BsonElement("reservationId")]
        public string ReservationId { get; set; } = string.Empty;

        [Required]
        [BsonElement("userId")]
        public long UserId { get; set; }

        [BsonElement("userNic")]
        public string? UserNic { get; set; }

        [Required]
        [BsonElement("stationName")]
        public string StationName { get; set; } = string.Empty;

        [Required]
        [BsonElement("vehicleNumber")]
        public string VehicleNumber { get; set; } = string.Empty;

        [Required]
        [BsonElement("vehicleModel")]
        public string VehicleModel { get; set; } = string.Empty;

        [BsonElement("batteryCapacity")]
        public string? BatteryCapacity { get; set; }

        [BsonElement("currentBattery")]
        public string? CurrentBattery { get; set; }

        [BsonElement("targetBattery")]
        public string? TargetBattery { get; set; }

        [Required]
        [BsonElement("contactNumber")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required]
        [BsonElement("reservationDate")]
        public string ReservationDate { get; set; } = string.Empty;

        [Required]
        [BsonElement("reservationTime")]
        public string ReservationTime { get; set; } = string.Empty;

        [Required]
        [BsonElement("duration")]
        public string Duration { get; set; } = string.Empty;

        [Required]
        [BsonElement("priority")]
        public string Priority { get; set; } = string.Empty;

        [Required]
        [BsonElement("paymentMethod")]
        public string PaymentMethod { get; set; } = string.Empty;

        [BsonElement("specialRequirements")]
        public string? SpecialRequirements { get; set; }

        [Required]
        [BsonElement("estimatedCost")]
        public string EstimatedCost { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.String)]
        [BsonElement("status")]
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

        [BsonElement("createdAt")]
        public string? CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public string? UpdatedAt { get; set; }

        [BsonElement("sid")]
        public long? Sid { get; set; }
    }
}


