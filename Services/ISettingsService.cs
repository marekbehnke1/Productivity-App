using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnAvalonia.Models;

namespace LearnAvalonia.Services
{
    public interface ISettingsService
    {
        SettingsModel LoadSettings();
        Task SaveSettingsAsync(SettingsModel settings);
        string SettingsFilePath { get; }
    }
}
