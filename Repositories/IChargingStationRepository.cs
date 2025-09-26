/**
 * EVChargingWebService - IChargingStationRepository
 * Interface for ChargingStation MongoDB CRUD operations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;

namespace EVChargingWebService.Repositories
{
    /// <summary>
    /// Abstraction for charging station persistence.
    /// </summary>
    public interface IChargingStationRepository
    {
        // Gets a station by id.
        Task<ChargingStation?> GetByIdAsync(string id);

        // Creates a new station.
        Task<ChargingStation> CreateAsync(ChargingStation station);

        // Updates an existing station.
        Task<bool> UpdateAsync(ChargingStation station);

        // Deletes a station by id.
        Task<bool> DeleteAsync(string id);

        // Lists all stations.
        Task<IReadOnlyList<ChargingStation>> GetAllAsync();
    }
}



