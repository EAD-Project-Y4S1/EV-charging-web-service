/**
 * EVChargingWebService - ChargingStationRepository
 * MongoDB repository implementation for ChargingStation.
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
    /// MongoDB repository for charging stations.
    /// </summary>
    public class ChargingStationRepository : IChargingStationRepository
    {
        private readonly IMongoCollection<ChargingStation> _stations;

        public ChargingStationRepository(IOptions<MongoDbSettings> mongoOptions)
        {
            // Initialize Mongo client and collection.
            var client = new MongoClient(mongoOptions.Value.ConnectionString);
            var database = client.GetDatabase(mongoOptions.Value.DatabaseName);
            _stations = database.GetCollection<ChargingStation>("stations");
        }

        public async Task<ChargingStation?> GetByIdAsync(string id)
        {
            // Retrieves station by id.
            return await _stations.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ChargingStation> CreateAsync(ChargingStation station)
        {
            // Inserts a new station.
            await _stations.InsertOneAsync(station);
            return station;
        }

        public async Task<bool> UpdateAsync(ChargingStation station)
        {
            // Replaces station document by id.
            var result = await _stations.ReplaceOneAsync(s => s.Id == station.Id, station);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            // Deletes station by id.
            var result = await _stations.DeleteOneAsync(s => s.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<ChargingStation>> GetAllAsync()
        {
            // Returns all stations.
            return await _stations.Find(Builders<ChargingStation>.Filter.Empty).ToListAsync();
        }
    }
}



