namespace FolderSyns.Core
{
    using FolderSyns.Interfaces;

    using System;
    using System.IO;

    public class LogManager : ILogManager
    {
        private string FILE_NAME = "Error.log";

        private readonly ISettingsManager _settingsManager;

        public LogManager(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public void SaveError(Exception ex)
        {
            using (StreamWriter sw = File.AppendText(Path.Combine(_settingsManager.FolderForHistory, FILE_NAME)))
            {
                sw.WriteLine($"{DateTime.Now:f}");
                sw.WriteLine($"{ex}");
                sw.WriteLine(string.Empty);
            }
        }
    }
}
