/**
 * EVChargingWebService - IUserRepository
 * Interface for User MongoDB CRUD operations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;

namespace EVChargingWebService.Repositories
{
    /// <summary>
    /// Abstraction for user persistence.
    /// </summary>
    public interface IUserRepository
    {
        // Gets a user by identifier.
        Task<User?> GetByIdAsync(string id);

        // Gets a user by email.
        Task<User?> GetByEmailAsync(string email);

        // Creates a new user.
        Task<User> CreateAsync(User user);

        // Updates an existing user.
        Task<bool> UpdateAsync(User user);

        // Deletes a user by id.
        Task<bool> DeleteAsync(string id);

        // Lists all users.
        Task<IReadOnlyList<User>> GetAllAsync();
    }
}


