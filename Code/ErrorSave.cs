namespace FolderSyns.Code
{
    using System;
    using System.IO;

    using FolderSyns.MVVM.SettingsUserControl;

    public static class ErrorSave
    {
        private static string FILE_NAME = "Error.log";

        public static void SaveError(Exception ex)
        {
            using (StreamWriter sw = File.AppendText(Path.Combine(SettingsModel.Inctance.FolderForHistory, FILE_NAME)))
            {
                sw.WriteLine($"{DateTime.Now:f}");
                sw.WriteLine($"{ex}");
                sw.WriteLine(string.Empty);
            }
        }
    }
}
