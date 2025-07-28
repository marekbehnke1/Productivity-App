using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnAvalonia.Models.Dtos;
using LearnAvalonia.Services;

namespace LearnAvalonia.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;
        
        [ObservableProperty]
        private string _passwordConfirm = string.Empty;

        [ObservableProperty]
        private bool _hasError = false;

        [ObservableProperty]
        private string? _errorMessage = string.Empty;

        [ObservableProperty]
        private double _windowHeight = 450;

        [ObservableProperty]
        private bool _isLoading = false;

        public EventHandler? RegisterSucceeded;
        public EventHandler? NavigateToLogin;

        public RegisterViewModel(IAuthenticationService authService)
        {
            _authService = authService;
        }

        // TODO: Add registration Logic
        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (Password != PasswordConfirm)
            {
                ShowError("Passwords did not match");
                return;
            }

            IsLoading = true;
            try
            {
                var registerRequest = new ApiRegisterRequest
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = Password
                };

                var request = await _authService.RegisterAsync(registerRequest);

                if (!request.Success)
                {
                    ShowError(request.Message ?? "Register failed");
                }
                else
                {
                    FirstName = string.Empty;
                    LastName = string.Empty;
                    Email = string.Empty;
                    Password = string.Empty;
                    PasswordConfirm = string.Empty;

                    RegisterSucceeded?.Invoke(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }

            return;
        }

        private void ShowError(string message)
        {
            HasError = true;
            ErrorMessage = message;
        }

        private void ClearError()
        {
            ErrorMessage = string.Empty;
            HasError = false;
        }

        [RelayCommand]
        private void NavigateToLoginView()
        {
            NavigateToLogin?.Invoke(this, EventArgs.Empty);
        }
    }
}
