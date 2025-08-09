using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LearnAvalonia.Models;

namespace LearnAvalonia.Services
{
    internal class SettingsService : ISettingsService
    {
        public string SettingsFilePath { get; }

        public SettingsService() 
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appDataPath, "LearnAvalonia");

            Directory.CreateDirectory(appFolder);

            SettingsFilePath = Path.Combine(appFolder, "settings.json");
        }

        public SettingsModel LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    var settings = JsonSerializer.Deserialize<SettingsModel>(json);
                    return settings ?? new SettingsModel(); 
                }
                else
                {
                    var defaultSettings = new SettingsModel();

                    _ = SaveSettingsAsync(defaultSettings);
                    return defaultSettings;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading settings: {ex.Message}");
                return new SettingsModel();
            }
        }

        public async Task SaveSettingsAsync(SettingsModel settings)
        {
            try
            {
                // make sure settings are saved as readable json
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(SettingsFilePath, json);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}
