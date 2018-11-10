namespace FolderSyns.Core
{
    using System;

    using System.Linq;
    using System.Configuration;

    using FolderSyns.Interfaces;

    /// <summary>
    /// Менеджер по работе с конфигурационным файлом.
    /// </summary>
    public class ConfigManager : IConfigManager
    {
        private readonly Configuration _configuration;
        private readonly AppSettingsSection _appSettingsSection;

        public ConfigManager()
        {
            _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _appSettingsSection = _configuration.AppSettings;
        }

        /// <summary>
        /// Загрузить значение по ключу.
        /// </summary>
        /// <param name="property">Ключ.</param>
        public T LoadSetting<T>(string property)
        {
            var result = _appSettingsSection.Settings.AllKeys.ToList().Contains(property)
                ? _appSettingsSection.Settings[property].Value
                : string.Empty;

            if (string.IsNullOrEmpty(result))
                return default(T);

            return (T)Convert.ChangeType(result, typeof(T));
        }

        /// <summary>
        /// Сохранить значение.
        /// </summary>
        /// <param name="property">Ключ.</param>
        /// <param name="path">Значение.</param>
        public void SaveFolderForHistory<T>(string property, T path)
        {
            if (_appSettingsSection.Settings.AllKeys.ToList().Contains(property))
                _appSettingsSection.Settings[property].Value = path.ToString();
            else
                _appSettingsSection.Settings.Add(property, path.ToString());
            _configuration.Save();
        }
    }
}
