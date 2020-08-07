using Core.Model.Settings;

namespace Core.Dal.Interfaces
{
    public interface ISettingsRepository
    {
        SettingsModel GetSettings();

        void SaveSettings(SettingsModel settingsModel);
    }
}