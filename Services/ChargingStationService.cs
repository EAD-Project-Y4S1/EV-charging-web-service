/**
 * EVChargingWebService - ChargingStationService
 * Implements business logic for charging stations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;
using EVChargingWebService.Repositories;

namespace EVChargingWebService.Services
{
    /// <summary>
    /// Provides charging station business operations.
    /// </summary>
    public class ChargingStationService : IChargingStationService
    {
        private readonly IChargingStationRepository _stationRepository;
        private readonly IBookingRepository _bookingRepository;

        public ChargingStationService(IChargingStationRepository stationRepository, IBookingRepository bookingRepository)
        {
            // Injects repositories.
            _stationRepository = stationRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<ChargingStation> CreateAsync(ChargingStation station)
        {
            // Creates a station with default active status.
            station.Status = ActiveStatus.Active;
            return await _stationRepository.CreateAsync(station);
        }

        public async Task<bool> UpdateAsync(ChargingStation station)
        {
            // Updates station details.
            var existing = await _stationRepository.GetByIdAsync(station.Id) ?? throw new KeyNotFoundException("Station not found");
            existing.Location = station.Location;
            existing.Type = station.Type;
            existing.SlotsAvailable = station.SlotsAvailable;
            existing.Schedule = station.Schedule;
            return await _stationRepository.UpdateAsync(existing);
        }

        public async Task<bool> UpdateScheduleAsync(string id, List<string> schedule)
        {
            // Updates station schedule only.
            var existing = await _stationRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Station not found");
            existing.Schedule = schedule;
            return await _stationRepository.UpdateAsync(existing);
        }

        public async Task<bool> ActivateAsync(string id)
        {
            // Activates station.
            var station = await _stationRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Station not found");
            station.Status = ActiveStatus.Active;
            return await _stationRepository.UpdateAsync(station);
        }

        public async Task<bool> DeactivateAsync(string id)
        {
            // Deactivates station if no active bookings.
            var station = await _stationRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Station not found");
            var activeCount = await _bookingRepository.CountActiveByStationAsync(id);
            if (activeCount > 0)
            {
                throw new InvalidOperationException("Cannot deactivate station with active bookings");
            }
            station.Status = ActiveStatus.Inactive;
            return await _stationRepository.UpdateAsync(station);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            // Deletes station by id.
            return await _stationRepository.DeleteAsync(id);
        }

        public async Task<ChargingStation?> GetByIdAsync(string id)
        {
            // Retrieves station by id.
            return await _stationRepository.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<ChargingStation>> GetAllAsync()
        {
            // Retrieves all stations.
            return await _stationRepository.GetAllAsync();
        }
    }
}


