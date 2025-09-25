/**
 * EVChargingWebService - UserRepository
 * MongoDB-based repository implementation for User entities.
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
    /// MongoDB repository for users.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IOptions<MongoDbSettings> mongoOptions)
        {
            // Initialize Mongo client and collection.
            var client = new MongoClient(mongoOptions.Value.ConnectionString);
            var database = client.GetDatabase(mongoOptions.Value.DatabaseName);
            _users = database.GetCollection<User>("users");

            // Ensure unique index on email.
            var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.Email);
            var indexModel = new CreateIndexModel<User>(indexKeys, new CreateIndexOptions { Unique = true });
            _users.Indexes.CreateOne(indexModel);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            // Retrieves a user by ObjectId string.
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // Retrieves a user by email.
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            // Inserts a new user document.
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            // Replaces user document by id.
            var result = await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            // Deletes user by id.
            var result = await _users.DeleteOneAsync(u => u.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            // Returns all users.
            return await _users.Find(Builders<User>.Filter.Empty).ToListAsync();
        }
    }
}


