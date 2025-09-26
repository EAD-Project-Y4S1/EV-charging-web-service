/**
 * EVChargingWebService - IBookingRepository
 * Interface for Booking MongoDB CRUD operations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;

namespace EVChargingWebService.Repositories
{
    /// <summary>
    /// Abstraction for booking persistence.
    /// </summary>
    public interface IBookingRepository
    {
        // Gets a booking by id.
        Task<Booking?> GetByIdAsync(string id);

        // Creates a booking.
        Task<Booking> CreateAsync(Booking booking);

        // Updates a booking.
        Task<bool> UpdateAsync(Booking booking);

        // Lists bookings by owner NIC.
        Task<IReadOnlyList<Booking>> GetByOwnerAsync(string ownerNIC);

        // Lists bookings by station id.
        Task<IReadOnlyList<Booking>> GetByStationAsync(string stationId);

        // Lists active bookings by station id.
        Task<int> CountActiveByStationAsync(string stationId);
    }
}



