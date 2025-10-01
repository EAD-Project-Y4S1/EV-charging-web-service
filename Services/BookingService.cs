/**
 * EVChargingWebService - BookingService
 * Implements business logic for bookings.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;
using EVChargingWebService.Repositories;

namespace EVChargingWebService.Services
{
    /// <summary>
    /// Provides booking business operations.
    /// </summary>
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEVOwnerRepository _ownerRepository;
        private readonly IChargingStationRepository _stationRepository;

        public BookingService(IBookingRepository bookingRepository, IEVOwnerRepository ownerRepository, IChargingStationRepository stationRepository)
        {
            // Injects repositories.
            _bookingRepository = bookingRepository;
            _ownerRepository = ownerRepository;
            _stationRepository = stationRepository;
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            // Validates business rules and creates a booking.
            var now = DateTime.UtcNow;
            if (booking.ReservationDateTime < now || booking.ReservationDateTime > now.AddDays(7))
            {
                throw new InvalidOperationException("Reservation must be within 7 days and in the future");
            }
            // var owner = await _ownerRepository.GetByNICAsync(booking.OwnerNIC) ?? throw new KeyNotFoundException("Owner not found");
            // if (owner.Status != OwnerStatus.Active)
            // {
            //     throw new InvalidOperationException("Owner is not active");
            // }
            var station = await _stationRepository.GetByIdAsync(booking.StationId) ?? throw new KeyNotFoundException("Station not found");
            if (station.Status != ActiveStatus.Active)
            {
                throw new InvalidOperationException("Station is not active");
            }
            booking.Status = BookingStatus.Active;
            return await _bookingRepository.CreateAsync(booking);
        }

        public async Task<bool> UpdateAsync(Booking booking)
        {
            // Updates a booking at least 12 hours before reservation.
            var existing = await _bookingRepository.GetByIdAsync(booking.Id) ?? throw new KeyNotFoundException("Booking not found");
            var now = DateTime.UtcNow;
            if ((existing.ReservationDateTime - now).TotalHours < 12)
            {
                throw new InvalidOperationException("Update allowed only at least 12 hours before reservation");
            }
            existing.StationId = booking.StationId;
            existing.ReservationDateTime = booking.ReservationDateTime;
            return await _bookingRepository.UpdateAsync(existing);
        }

        public async Task<bool> CancelAsync(string id)
        {
            // Cancels a booking at least 12 hours before reservation.
            var existing = await _bookingRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Booking not found");
            var now = DateTime.UtcNow;
            if ((existing.ReservationDateTime - now).TotalHours < 12)
            {
                throw new InvalidOperationException("Cancel allowed only at least 12 hours before reservation");
            }
            existing.Status = BookingStatus.Cancelled;
            return await _bookingRepository.UpdateAsync(existing);
        }

        public async Task<IReadOnlyList<Booking>> GetByOwnerAsync(string nic)
        {
            // Returns bookings for owner.
            return await _bookingRepository.GetByOwnerAsync(nic);
        }

        public async Task<IReadOnlyList<Booking>> GetByStationAsync(string stationId)
        {
            // Returns bookings for station.
            return await _bookingRepository.GetByStationAsync(stationId);
        }

        public async Task<Booking?> GetByIdAsync(string id)
        {
            // Returns booking by id.
            return await _bookingRepository.GetByIdAsync(id);
        }
    }
}


