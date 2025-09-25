/**
 * EVChargingWebService - IUserService
 * Business logic contract for user operations.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using EVChargingWebService.Models;

namespace EVChargingWebService.Services
{
    /// <summary>
    /// Contract for user business operations.
    /// </summary>
    public interface IUserService
    {
        // Creates a user with hashed password.
        Task<User> CreateAsync(string email, string password, string fullName, UserRole role);

        // Updates user details (not password).
        Task<bool> UpdateAsync(string id, string email, string fullName, UserRole role, bool isActive);

        // Deletes a user.
        Task<bool> DeleteAsync(string id);

        // Gets a user by id.
        Task<User?> GetByIdAsync(string id);

        // Lists all users.
        Task<IReadOnlyList<User>> GetAllAsync();

        // Validates credentials and returns user if valid.
        Task<User?> ValidateCredentialsAsync(string email, string password);
    }
}


