/**
 * EVChargingWebService - IEVOwnerRepository
 * Interface for EVOwner MongoDB CRUD operations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;

namespace EVChargingWebService.Repositories
{
    /// <summary>
    /// Abstraction for EV owner persistence.
    /// </summary>
    public interface IEVOwnerRepository
    {
        // Gets an EV owner by NIC.
        Task<EVOwner?> GetByNICAsync(string nic);

        // Creates a new EV owner.
        Task<EVOwner> CreateAsync(EVOwner owner);

        // Updates an existing EV owner.
        Task<bool> UpdateAsync(EVOwner owner);

        // Deletes an EV owner by NIC.
        Task<bool> DeleteAsync(string nic);

        // Lists all EV owners.
        Task<IReadOnlyList<EVOwner>> GetAllAsync();
    }
}



