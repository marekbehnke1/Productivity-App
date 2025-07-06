using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using LearnAvalonia.Models;
using LearnAvalonia.Models.Dtos;
using LearnAvalonia.Exceptions;
using System.Reflection.Metadata.Ecma335;

namespace LearnAvalonia.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        // private readonly properties
        // These are the private properties we can change within the Authservice
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorage _secureStorage;

        private UserDto? _currentUser;
        private string? _currentToken;
        private bool _isAuthenticated;

        // public accesible properties
        // These are the Get properties from the interface
        public UserDto? CurrentUser => _currentUser;
        public string? CurrentToken => _currentToken;
        public bool IsAuthenticated => _isAuthenticated;

        // Auth state event handler
        public event EventHandler <AuthStateChangedEventArgs>? AuthStateChanged;
        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_secureStorage = secureStorage;
        }

        public async Task<AuthResponse> LoginAsync(ApiLoginRequest request)
        {
            try
            {
                // gets response from login endpoint
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

                // Handle any errors that might give us a fail code
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // Invalid Credentials
                        return new AuthResponse
                        {
                            Success = false,
                            User = null,
                            Token = string.Empty,
                            Message = "Invalid email or password"
                        };
                    }
                    else
                    {
                        // server or network error
                        throw new ApiException($"Login failed with status: {response.StatusCode}: {errorContent}");
                    }
                }

                // attempts to parse the response object as json
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse == null)
                {
                    throw new ApiException("Invalid response from server");
                }

                // Validate the response data
                if (string.IsNullOrEmpty(authResponse.Token) || authResponse.User == null)
                {
                    throw new ApiException("Invalid auth response, missing user or token");
                }

                // Error checks passed - set properties.
                _currentUser = authResponse.User;
                _currentToken = authResponse.Token;
                _isAuthenticated = true;

                // Fire status change event
                AuthStateChanged?.Invoke(this, new AuthStateChangedEventArgs(
                    user: _currentUser,
                    changeReason: AuthChangeReason.Login,
                    isAuthenticated: _isAuthenticated
                    ));
                
                // update authResponse
                authResponse.Success = true;

                // As everything is fine - update and return the origninal auth response
                return authResponse;
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException($"Cannot connect to server: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new ApiException($"Server return invalid response: {ex.Message}");
            }
        }

        public async Task LogoutAsync()
        {
            var previousUser = _currentUser;

            _currentUser = null;
            _currentToken = null;
            _isAuthenticated = false;

            AuthStateChanged?.Invoke(this, new AuthStateChangedEventArgs(
                user: previousUser,
                changeReason: AuthChangeReason.Logout,
                isAuthenticated: _isAuthenticated
                ));

            await Task.CompletedTask;
        }

    }
}
