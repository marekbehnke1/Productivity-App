using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnAvalonia.Models.Dtos;
using System.Threading.Tasks;

namespace LearnAvalonia.Services
{
    /// <summary>
    /// Provides authentication services for the application
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Gets a value indicating whether the user is currently authenticated
        /// </summary>
        public bool IsAuthenticated { get; }
        /// <summary>
        /// Gets an object representing the current user
        /// </summary>
        public UserDto? CurrentUser { get; }
        /// <summary>
        /// Gets a string representing the current authentication token
        /// </summary>
        public string? CurrentToken { get; }

        /// <summary>
        /// Authenticates a user with the provided credentials
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns>Authentication response with token and user info</returns>
        /// <exception cref="NetworkException">Thrown when unable to reach authentication server</exception>
        /// <exception cref="ApiException">Thrown when API returns unexpected error</exception>
        public Task<AuthResponse> LoginAsync(ApiLoginRequest request);

        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <param name="request">Registration information</param>
        /// <returns>Authentication response with token and user info</returns>
        /// <exception cref="NetworkException">Thrown when unable to reach authentication server</exception>
        /// <exception cref="ApiException">Thrown when API returns unexpected error</exception>
        public Task<AuthResponse> RegisterAsync(ApiRegisterRequest request);

        /// <summary>
        /// Logs out the current user and clears authentication state
        /// </summary>
        public Task LogoutAsync();

        /// <summary>
        /// Initializes authentication state on application startup
        /// </summary>
        public Task InitializeAuthAsync();

        /// <summary>
        /// Handles unauthorized responses from the server
        /// </summary>
        void HandleUnauthorized();

        /// <summary>
        /// Raised when authentication state changes (login, logout, token expiry)
        /// </summary>
        event EventHandler<AuthStateChangedEventArgs> AuthStateChanged;

    }
}
