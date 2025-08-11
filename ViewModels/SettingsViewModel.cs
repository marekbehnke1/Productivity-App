using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using LearnAvalonia.Services;
using LearnAvalonia.Models;

namespace LearnAvalonia.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {

        private readonly ISettingsService _settingsService;
        private SettingsModel _currentSettings;


        [ObservableProperty]
        private double _windowHeight = 800;

        [ObservableProperty]
        private int _timerWorkDuration; 

        [ObservableProperty]
        private int _timerBreakDuration; 

        //Events
        public event EventHandler? NavigateToMainViewModel;

        public event EventHandler? SettingsUpdated;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _currentSettings = new SettingsModel();
            LoadSettings();

        }

        private void LoadSettings()
        {
            _currentSettings = _settingsService.LoadSettings();
            TimerWorkDuration = _currentSettings.WorkDuration;
            TimerBreakDuration = _currentSettings.BreakDuration;
        }

        [RelayCommand]
        private async Task SaveSettingsAsync()
        {
            _currentSettings.WorkDuration = TimerWorkDuration;
            _currentSettings.BreakDuration = TimerBreakDuration;

            await _settingsService.SaveSettingsAsync(_currentSettings);

            SettingsUpdated?.Invoke(this, EventArgs.Empty);

            Debug.WriteLine("Settings Saved");
        }

        [RelayCommand]
        private void SwitchToMainView()
        {
            NavigateToMainViewModel?.Invoke(this, EventArgs.Empty);
        }

        
    }
}
