using Core.Model.Settings;

namespace Core.Manager.Settings.Interface
{
    public interface ISettingsManager
    {
        SettingsModel SettingsModel { get; }
        void SaveSettings();
    }
}