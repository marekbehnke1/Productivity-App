using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnAvalonia.Models.Dtos;
using LearnAvalonia.Services;
using Tmds.DBus.Protocol;

namespace LearnAvalonia.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;

        // Form fields
        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        // UI State
        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _hasError = false;

        [ObservableProperty]
        private double _windowHeight = 300;

        //Events for navigation
        public event EventHandler? LoginSucceeded;
        public event EventHandler? NavigateToRegister;

        public LoginViewModel(IAuthenticationService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            //TODO add validation
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ShowError("Please enter both email and password");
                return;
            }

            try
            {
                IsLoading = true;
                ClearError();

                var loginRequest = new ApiLoginRequest
                {
                    Email = Email,
                    Password = Password
                };

                var response = await _authService.LoginAsync(loginRequest);

                if (response.Success)
                {
                    //Clear form
                    Email = string.Empty;
                    Password = string.Empty;

                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ShowError(response.Message ?? "Login Failed");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Login error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void NavigateToRegisterView()
        {
            NavigateToRegister?.Invoke(this, EventArgs.Empty);
        }
        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;
        }
        private void ClearError()
        {
            ErrorMessage = string.Empty;
            HasError = false;
        }
    }
}
