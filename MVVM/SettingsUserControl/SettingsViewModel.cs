using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FolderSyns.Code.Helpers;

namespace FolderSyns.MVVM.SettingsUserControl
{
    public class SettingsViewModel
    {
        private string _folderForHistory;

        public string FolderForHistory
        {
            get => _folderForHistory;
            set => PropertyHelper.SetProperty(ref _folderForHistory, value, this, nameof(FolderForHistory));
        }

        public RelayCommand SaveCommand { get; }

        public static SettingsViewModel Inctance { get; } = new SettingsViewModel();

        private SettingsViewModel()
        {
            FolderForHistory = SettingsModel.Inctance.FolderForHistory;
            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            ControlSettings.Instance.SaveFolderForHistory(nameof(SettingsModel.FolderForHistory), FolderForHistory);
        }
    }
}

