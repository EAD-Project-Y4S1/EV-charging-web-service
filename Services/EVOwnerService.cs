/**
 * EVChargingWebService - EVOwnerService
 * Implements business logic for EV owners.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;
using EVChargingWebService.Repositories;

namespace EVChargingWebService.Services
{
    /// <summary>
    /// Provides EV owner business operations.
    /// </summary>
    public class EVOwnerService : IEVOwnerService
    {
        private readonly IEVOwnerRepository _ownerRepository;

        public EVOwnerService(IEVOwnerRepository ownerRepository)
        {
            // Injects repository.
            _ownerRepository = ownerRepository;
        }

        public async Task<EVOwner> CreateAsync(EVOwner owner)
        {
            // Ensures NIC uniqueness and creates owner.
            if (string.IsNullOrWhiteSpace(owner.NIC))
            {
                throw new ArgumentException("NIC is required");
            }
            var existing = await _ownerRepository.GetByNICAsync(owner.NIC);
            if (existing != null)
            {
                throw new InvalidOperationException("NIC already exists");
            }
            owner.Status = OwnerStatus.Active;
            return await _ownerRepository.CreateAsync(owner);
        }

        public async Task<bool> UpdateAsync(EVOwner owner)
        {
            // Updates owner details.
            var existing = await _ownerRepository.GetByNICAsync(owner.NIC) ?? throw new KeyNotFoundException("Owner not found");
            existing.Name = owner.Name;
            existing.Email = owner.Email;
            existing.Phone = owner.Phone;
            existing.VehicleDetails = owner.VehicleDetails;
            existing.Status = owner.Status;
            return await _ownerRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(string nic)
        {
            // Deletes owner by NIC.
            return await _ownerRepository.DeleteAsync(nic);
        }

        public async Task<bool> ActivateAsync(string nic)
        {
            // Activates owner account.
            var owner = await _ownerRepository.GetByNICAsync(nic) ?? throw new KeyNotFoundException("Owner not found");
            owner.Status = OwnerStatus.Active;
            return await _ownerRepository.UpdateAsync(owner);
        }

        public async Task<bool> DeactivateAsync(string nic)
        {
            // Deactivates owner account.
            var owner = await _ownerRepository.GetByNICAsync(nic) ?? throw new KeyNotFoundException("Owner not found");
            owner.Status = OwnerStatus.Inactive;
            return await _ownerRepository.UpdateAsync(owner);
        }

        public async Task<EVOwner?> GetByNICAsync(string nic)
        {
            // Retrieves owner by NIC.
            return await _ownerRepository.GetByNICAsync(nic);
        }

        public async Task<IReadOnlyList<EVOwner>> GetAllAsync()
        {
            // Retrieves all owners.
            return await _ownerRepository.GetAllAsync();
        }
    }
}


