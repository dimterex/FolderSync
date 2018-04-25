using System.Configuration;
using System.Linq;
using FolderSyns.Code.Helpers;

namespace FolderSyns.MVVM.SettingsUserControl
{
    public class SettingsModel
    {
       
        public string FolderForHistory { get; private set; }

        public string DefaultSourceFolder { get; private set; }

        public static SettingsModel Inctance { get; } = new SettingsModel();

        private SettingsModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            FolderForHistory = ControlSettings.Instance.LoadSetting(nameof(FolderForHistory));
            DefaultSourceFolder = ControlSettings.Instance.LoadSetting(nameof(DefaultSourceFolder));
        }

    
    }
}
