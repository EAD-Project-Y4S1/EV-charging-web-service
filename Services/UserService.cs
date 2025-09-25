/**
 * EVChargingWebService - UserService
 * Implements business logic for user management.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

using BCrypt.Net;
using EVChargingWebService.Models;
using EVChargingWebService.Repositories;

namespace EVChargingWebService.Services
{
    /// <summary>
    /// Provides user-related business operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            // Injects repository.
            _userRepository = userRepository;
        }

        public async Task<User> CreateAsync(string email, string password, string fullName, UserRole role)
        {
            // Creates a new user with hashed password.
            var existing = await _userRepository.GetByEmailAsync(email);
            if (existing != null)
            {
                throw new InvalidOperationException("Email already in use");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FullName = fullName,
                Role = role,
                IsActive = true
            };
            return await _userRepository.CreateAsync(user);
        }

        public async Task<bool> UpdateAsync(string id, string email, string fullName, UserRole role, bool isActive)
        {
            // Updates fields except password.
            var user = await _userRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("User not found");
            user.Email = email;
            user.FullName = fullName;
            user.Role = role;
            user.IsActive = isActive;
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            // Deletes user by id.
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            // Retrieves user by id.
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            // Retrieves all users.
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> ValidateCredentialsAsync(string email, string password)
        {
            // Validates email/password and returns user if valid.
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !user.IsActive)
            {
                return null;
            }
            var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return valid ? user : null;
        }
    }
}


