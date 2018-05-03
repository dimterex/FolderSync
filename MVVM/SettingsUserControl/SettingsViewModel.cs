namespace FolderSyns.MVVM.SettingsUserControl
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FolderSyns.Annotations;
    using FolderSyns.Code.Helpers;

    public class SettingsViewModel : INotifyPropertyChanged
    {
        #region Fields
        private string _folderForHistory;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Fields

        #region Properties
        public string FolderForHistory
        {
            get => _folderForHistory;
            set
            {
                _folderForHistory = value;
                OnPropertyChanged(nameof(FolderForHistory));
            }
        }
        public RelayCommand SaveCommand { get; }
        public RelayCommand OpenFolderCommand { get; }

        public static SettingsViewModel Inctance { get; } = new SettingsViewModel();
        #endregion Properties

        private SettingsViewModel()
        {
            FolderForHistory = SettingsModel.Inctance.FolderForHistory;
            SaveCommand = new RelayCommand(Save);
            OpenFolderCommand = new RelayCommand(OpenFolderPath);
        }

        #region Methods
        /// <summary>
        /// Указать папку.
        /// </summary>
        /// <param name="param">Указывается первая папка.</param>
        private void OpenFolderPath(object param)
        {
            if (!Enum.TryParse(param.ToString(), out FolderType folderType))
                return;

            switch (folderType)
            {
                case FolderType.FolderForHistory:
                    FolderForHistory = OpenFolderDialog.OpenFolderPath(FolderForHistory);
                    return;
            }
        }

        private void Save()
        {
            ControlSettings.Instance.SaveFolderForHistory(nameof(SettingsModel.FolderForHistory), FolderForHistory);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion Methods
    }
}

