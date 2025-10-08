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
            var reservationDateTime = DateTime.Parse($"{booking.ReservationDate} {booking.ReservationTime}");
            if (reservationDateTime < now || reservationDateTime > now.AddDays(7))
            {
                throw new InvalidOperationException("Reservation must be within 7 days and in the future");
            }
            // var owner = await _ownerRepository.GetByNICAsync(booking.UserNic) ?? throw new KeyNotFoundException("Owner not found");
            // if (owner.Status != OwnerStatus.Active)
            // {
            //     throw new InvalidOperationException("Owner is not active");
            // }
            // Validate that the station name is not empty
            if (string.IsNullOrWhiteSpace(booking.StationName))
            {
                throw new InvalidOperationException("Station name is required");
            }
            
            // Note: Since we're storing station names as strings, we can't validate against a specific station
            // If you need to validate against existing stations, you would need to add a method to find stations by name
            // or modify the ChargingStation model to include a Name field
            booking.Status = BookingStatus.Confirmed;
                return await _bookingRepository.CreateAsync(booking);
        }

        public async Task<bool> UpdateAsync(Booking booking)
        {
            // Updates a booking at least 12 hours before reservation.
            var existing = await _bookingRepository.GetByIdAsync(booking.Id) ?? throw new KeyNotFoundException("Booking not found");
            var now = DateTime.UtcNow;
            if (DateTime.TryParse($"{existing.ReservationDate} {existing.ReservationTime}", out var existingDateTime))
            {
                if ((existingDateTime - now).TotalHours < 12)
                {
                    throw new InvalidOperationException("Update allowed only at least 12 hours before reservation");
                }
            }
            existing.StationName = booking.StationName;
            existing.ReservationDate = booking.ReservationDate;
            existing.ReservationTime = booking.ReservationTime;
            return await _bookingRepository.UpdateAsync(existing);
        }

        public async Task<bool> CancelAsync(string id)
        {
            // Cancels a booking at least 12 hours before reservation.
            var existing = await _bookingRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Booking not found");
            var now = DateTime.UtcNow;
            if (DateTime.TryParse($"{existing.ReservationDate} {existing.ReservationTime}", out var existingDateTime))
            {
                if ((existingDateTime - now).TotalHours < 12)
                {
                    throw new InvalidOperationException("Cancel allowed only at least 12 hours before reservation");
                }
            }
            existing.Status = BookingStatus.Cancelled;
            return await _bookingRepository.UpdateAsync(existing);
        }

        public async Task<IReadOnlyList<Booking>> GetByOwnerAsync(string nic)
        {
            // Returns bookings for owner.
            return await _bookingRepository.GetByOwnerAsync(nic);
        }

        public async Task<IReadOnlyList<Booking>> GetByStationAsync(string stationName)
        {
            // Returns bookings for station by name.
            return await _bookingRepository.GetByStationAsync(stationName);
        }

        public async Task<Booking?> GetByIdAsync(string id)
        {
            // Returns booking by id.
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<Booking>> GetAllAsync()
        {
            // Returns all bookings.
            return await _bookingRepository.GetAllAsync();
        }
    }
}


