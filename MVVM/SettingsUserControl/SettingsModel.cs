using System.Configuration;
using System.Linq;
using FolderSyns.Code.Helpers;

namespace FolderSyns.MVVM.SettingsUserControl
{
    public class SettingsModel
    {
        #region Properties
        public string FolderForHistory { get; private set; }

        public string DefaultSourceFolder { get; private set; }

        public string DefaultTargetFolder { get; private set; }

        public static SettingsModel Inctance { get; } = new SettingsModel();
        #endregion Properties

        private SettingsModel()
        {
            Initialize();
        }

        #region Methods
        private void Initialize()
        {
            FolderForHistory = ControlSettings.Instance.LoadSetting(nameof(FolderForHistory));
            DefaultSourceFolder = ControlSettings.Instance.LoadSetting(nameof(DefaultSourceFolder));
            DefaultTargetFolder = ControlSettings.Instance.LoadSetting(nameof(DefaultTargetFolder));
        }

        public void SetDefaultSourceFolder(string sourceFolder)
        {
            if (DefaultSourceFolder.Equals(sourceFolder))
                return;

            DefaultSourceFolder = sourceFolder;
            ControlSettings.Instance.SaveFolderForHistory(nameof(DefaultSourceFolder), sourceFolder);
        }

        public void SetDefaultTargetFolder(string targetFolder)
        {
            if (DefaultTargetFolder.Equals(targetFolder))
                return;

            DefaultTargetFolder = targetFolder;
            ControlSettings.Instance.SaveFolderForHistory(nameof(DefaultTargetFolder), targetFolder);
        }
        #endregion Methods
    }
}
