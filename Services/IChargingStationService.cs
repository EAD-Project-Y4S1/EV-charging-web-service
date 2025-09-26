/**
 * EVChargingWebService - IChargingStationService
 * Business logic contract for charging stations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;

namespace EVChargingWebService.Services
{
    /// <summary>
    /// Contract for charging station business operations.
    /// </summary>
    public interface IChargingStationService
    {
        // Creates a station.
        Task<ChargingStation> CreateAsync(ChargingStation station);

        // Updates station details.
        Task<bool> UpdateAsync(ChargingStation station);

        // Updates station schedule.
        Task<bool> UpdateScheduleAsync(string id, List<string> schedule);

        // Activates a station.
        Task<bool> ActivateAsync(string id);

        // Deactivates a station (only if no active bookings).
        Task<bool> DeactivateAsync(string id);

        // Deletes a station.
        Task<bool> DeleteAsync(string id);

        // Gets a station by id.
        Task<ChargingStation?> GetByIdAsync(string id);

        // Lists all stations.
        Task<IReadOnlyList<ChargingStation>> GetAllAsync();
    }
}


