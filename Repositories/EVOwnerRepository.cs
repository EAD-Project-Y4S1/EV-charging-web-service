/**
 * EVChargingWebService - EVOwnerRepository
 * MongoDB repository implementation for EV owners.
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
    /// MongoDB repository for EV owners.
    /// </summary>
    public class EVOwnerRepository : IEVOwnerRepository
    {
        private readonly IMongoCollection<EVOwner> _owners;

        public EVOwnerRepository(IOptions<MongoDbSettings> mongoOptions)
        {
            // Initialize Mongo client and collection.
            var client = new MongoClient(mongoOptions.Value.ConnectionString);
            var database = client.GetDatabase(mongoOptions.Value.DatabaseName);
            _owners = database.GetCollection<EVOwner>("evowners");

            // No need to create index on NIC - it's the _id field which is automatically unique
            // MongoDB automatically creates a unique index on _id for every collection
        }
        public async Task<EVOwner?> GetByNICAsync(string nic)
        {
            // Retrieves an owner by NIC.
            return await _owners.Find(o => o.NIC == nic).FirstOrDefaultAsync();
        }

        public async Task<EVOwner> CreateAsync(EVOwner owner)
        {
            // Inserts a new owner document.
            await _owners.InsertOneAsync(owner);
            return owner;
        }

        public async Task<bool> UpdateAsync(EVOwner owner)
        {
            // Replaces owner document by NIC.
            var result = await _owners.ReplaceOneAsync(o => o.NIC == owner.NIC, owner);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string nic)
        {
            // Deletes owner by NIC.
            var result = await _owners.DeleteOneAsync(o => o.NIC == nic);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<EVOwner>> GetAllAsync()
        {
            // Returns all owners.
            return await _owners.Find(Builders<EVOwner>.Filter.Empty).ToListAsync();
        }
    }
}



