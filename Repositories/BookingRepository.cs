/**
 * EVChargingWebService - BookingRepository
 * MongoDB repository implementation for bookings.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Config;
using EVChargingWebService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EVChargingWebService.Repositories
{
    /// <summary>
    /// MongoDB repository for bookings.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly IMongoCollection<Booking> _bookings;

        public BookingRepository(IOptions<MongoDbSettings> mongoOptions)
        {
            // Initialize Mongo client and collection.
            var client = new MongoClient(mongoOptions.Value.ConnectionString);
            var database = client.GetDatabase(mongoOptions.Value.DatabaseName);
            _bookings = database.GetCollection<Booking>("bookings");
        }

        public async Task<Booking?> GetByIdAsync(string id)
        {
            // Retrieves a booking by id.
            return await _bookings.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            // Inserts a new booking.
            await _bookings.InsertOneAsync(booking);
            return booking;
        }

        public async Task<bool> UpdateAsync(Booking booking)
        {
            // Replaces booking document by id.
            var result = await _bookings.ReplaceOneAsync(b => b.Id == booking.Id, booking);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<IReadOnlyList<Booking>> GetByOwnerAsync(string ownerNIC)
        {
            // Lists bookings for a specific owner.
            return await _bookings.Find(b => b.OwnerNIC == ownerNIC).ToListAsync();
        }

        public async Task<IReadOnlyList<Booking>> GetByStationAsync(string stationId)
        {
            // Lists bookings for a specific station.
            return await _bookings.Find(b => b.StationId == stationId).ToListAsync();
        }

        public async Task<int> CountActiveByStationAsync(string stationId)
        {
            // Counts active bookings for a station.
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.StationId, stationId),
                Builders<Booking>.Filter.Eq(b => b.Status, BookingStatus.Active)
            );
            return (int)await _bookings.CountDocumentsAsync(filter);
        }
    }
}



