using Core.Dal.Interfaces;
using Core.Manager.Settings.Interface;
using Core.Model.Settings;

namespace Core.Manager.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsManager(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            SettingsModel = _settingsRepository.GetSettings();
        }

        public SettingsModel SettingsModel { get; }

        public void SaveSettings()
        {
            _settingsRepository.SaveSettings(SettingsModel);
        }
    }
}