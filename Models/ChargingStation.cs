/**
 * EVChargingWebService - ChargingStation Model
 * Represents a charging station with schedule and status.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EVChargingWebService.Models
{
    /// <summary>
    /// Charging station type.
    /// </summary>
    public enum StationType
    {
        AC = 1,
        DC = 2
    }

    /// <summary>
    /// Generic active/inactive status.
    /// </summary>
    public enum ActiveStatus
    {
        Inactive = 0,
        Active = 1
    }

    /// <summary>
    /// Charging station entity stored in MongoDB.
    /// </summary>
    public class ChargingStation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [BsonElement("location")]
        public string Location { get; set; } = string.Empty;

        [Required]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("type")]
        public StationType Type { get; set; }

        [Range(0, int.MaxValue)]
        [BsonElement("slotsAvailable")]
        public int SlotsAvailable { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("status")]
        public ActiveStatus Status { get; set; } = ActiveStatus.Active;

        // Simple schedule representation: list of human-readable time windows.
        [BsonElement("schedule")]
        public List<string> Schedule { get; set; } = new List<string>();
    }
}


