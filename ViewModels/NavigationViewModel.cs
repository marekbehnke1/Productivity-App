using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using LearnAvalonia.Services;
using LearnAvalonia.Models.Dtos;

namespace LearnAvalonia.ViewModels
{
    public partial class NavigationViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ITaskService _taskService;

        // Active ViewModel

        [ObservableProperty]
        private ViewModelBase _currentViewModel;

        [ObservableProperty]
        private LoginViewModel _loginViewModel;

        [ObservableProperty]
        private RegisterViewModel _registerViewModel;

        [ObservableProperty]
        private MainViewModel _mainViewModel;
        
        [ObservableProperty]
        private SettingsViewModel _settingsViewModel;

        public NavigationViewModel(IAuthenticationService authService, ITaskService taskService, MainViewModel mainViewModel, SettingsViewModel settingsViewModel)
        {
            _authService = authService;
            _taskService = taskService;
            _mainViewModel = mainViewModel;
            _settingsViewModel = settingsViewModel;

            _loginViewModel = new LoginViewModel(_authService);
            _registerViewModel = new RegisterViewModel(_authService);

            // Wire up events
            _loginViewModel.LoginSucceeded += OnLoginSucceeded;
            _loginViewModel.NavigateToRegister += OnNavigateToRegister;
            _registerViewModel.RegisterSucceeded += OnRegisterSucceeded;
            _registerViewModel.NavigateToLogin += OnNavigateToLogin;

            _authService.AuthStateChanged += OnAuthStateChanged;

            // Set initial value based on auth state
            _currentViewModel = _authService.IsAuthenticated ? _mainViewModel : _loginViewModel;

        }

        private void OnLoginSucceeded(object? sender, EventArgs e)
        {
            Debug.WriteLine("Login Succeeded - navigation happens by auth state changed");
        }
        private void OnRegisterSucceeded(object? sender, EventArgs e)
        {
            Debug.WriteLine("Login Succeeded - navigation happens by auth state changed");
        }
        private void OnNavigateToRegister(object? sender, EventArgs e)
        {
            CurrentViewModel = RegisterViewModel;
        }
        private void OnNavigateToLogin(object? sender, EventArgs e)
        {
            CurrentViewModel = LoginViewModel;
        }
        private void OnAuthStateChanged(object? sender, AuthStateChangedEventArgs e)
        {
            Debug.WriteLine($"Navigation auth state changed - IsAuth: {e.IsAuthenticated}");

            if (e.IsAuthenticated)
            {
                CurrentViewModel = MainViewModel;
            }
            else
            {
                CurrentViewModel= LoginViewModel;
            }
        }
        
    }
}
