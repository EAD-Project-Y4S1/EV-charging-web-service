/**
 * EVChargingWebService - IEVOwnerService
 * Business logic contract for EV owner operations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;

namespace EVChargingWebService.Services
{
    /// <summary>
    /// Contract for EV owner operations.
    /// </summary>
    public interface IEVOwnerService
    {
        // Creates a new EV owner.
        Task<EVOwner> CreateAsync(EVOwner owner);

        // Updates an EV owner.
        Task<bool> UpdateAsync(EVOwner owner);

        // Deletes an EV owner.
        Task<bool> DeleteAsync(string nic);

        // Activates an EV owner account.
        Task<bool> ActivateAsync(string nic);

        // Deactivates an EV owner account.
        Task<bool> DeactivateAsync(string nic);

        // Gets an EV owner by NIC.
        Task<EVOwner?> GetByNICAsync(string nic);

        // Lists all EV owners.
        Task<IReadOnlyList<EVOwner>> GetAllAsync();
    }
}


