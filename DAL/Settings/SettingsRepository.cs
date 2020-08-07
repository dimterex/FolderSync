using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Core.Dal.Interfaces;
using Core.Model.Settings;
using DAL.DTO;
using Newtonsoft.Json;

namespace DAL
{
    public class SettingsRepository : ISettingsRepository
    {
        private const string SETTINGS_FILE_NAME = "settings.json";
      

        private string _path;

        public SettingsRepository()
        {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _path = Path.Combine(directory, SETTINGS_FILE_NAME);
        }

        public SettingsModel GetSettings()
        {
            if (!File.Exists(_path))
                return new SettingsModel();
            
            var settings = new SettingsModel();
            var rawJson = File.ReadAllText(_path);
            SettingsDto settingsDto = JsonConvert.DeserializeObject<SettingsDto>(rawJson);
            settings.FolderForHistory = settingsDto.FolderForHistory;
            settings.IsUseFilter = settingsDto.IsUseFilter;
            settings.IsUseIgnoreFilter = settingsDto.IsUseIgnoreFilter;
            settings.DefaultSourceFolder = settingsDto.DefaultSourceFolder;
            settings.DefaultTargetFolder = settingsDto.DefaultTargetFolder;
            settings.IgnorableFileFormat = settingsDto.IgnorableFileFormat;
            settings.FilteredFileFormat = settingsDto.FilteredFileFormat;
            settings.CultureInfo = new CultureInfo(settingsDto.Locale);
            return settings;
        }

        public void SaveSettings(SettingsModel settings)
        {
            var settingsDto = new SettingsDto();
            
            settingsDto.FolderForHistory = settings.FolderForHistory;
            settingsDto.IsUseFilter = settings.IsUseFilter;
            settingsDto.IsUseIgnoreFilter = settings.IsUseIgnoreFilter;
            settingsDto.DefaultSourceFolder = settings.DefaultSourceFolder;
            settingsDto.DefaultTargetFolder = settings.DefaultTargetFolder;
            settingsDto.IgnorableFileFormat = settings.IgnorableFileFormat;
            settingsDto.FilteredFileFormat = settings.FilteredFileFormat;
            settingsDto.Locale = settings.CultureInfo.Name;
            
            string json = JsonConvert.SerializeObject(settingsDto, Formatting.Indented);
            File.WriteAllText(_path, json);
        }
    }
}