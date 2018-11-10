namespace FolderSyns.ViewModels
{
    using FolderSyns.Code;
    using FolderSyns.Core;
    using FolderSyns.Enums;
    using FolderSyns.Interfaces;

    using Prism.Mvvm;

    using System;

    public class SettingsViewModel : BindableBase
    {
        #region Fields

        private string _folderForHistory;

        private readonly IConfigManager _configManager;
        private readonly IGetFolderManager _getFolderManager;
        private readonly ISettingsManager _settingsManager;
        private string _filteredFileFormat;
        private string _ignorableFileFormat;
        private bool _IsUseIgnoreFillter;
        private bool _IsUseFillter;

        #endregion Fields

        #region Properties

        public string FolderForHistory
        {
            get => _folderForHistory;
            set => SetProperty(ref _folderForHistory, value);
        }

        public string FilteredFileFormat
        {
            get => _filteredFileFormat;
            set => SetProperty(ref _filteredFileFormat, value);
        }

        public string IgnorableFileFormat
        {
            get => _ignorableFileFormat;
            set => SetProperty(ref _ignorableFileFormat, value);
        }

        public bool IsUseFillter
        {
            get => _IsUseFillter;
            set => SetProperty(ref _IsUseFillter, value);
        }

        public bool IsUseIgnoreFillter
        {
            get => _IsUseIgnoreFillter;
            set => SetProperty(ref _IsUseIgnoreFillter, value);
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand OpenFolderCommand { get; }

        #endregion Properties

        public SettingsViewModel(ISettingsManager settingsManager, IConfigManager configManager, IGetFolderManager getFolderManager)
        {
            _settingsManager = settingsManager;
            _configManager = configManager;
            _getFolderManager = getFolderManager;

            FolderForHistory = settingsManager.FolderForHistory;
            IsUseFillter = settingsManager.IsUseFillter;
            IsUseIgnoreFillter = settingsManager.IsUseIgnoreFillter;

            FilteredFileFormat = string.Join(SettingsManager.SEPARATOR.ToString(), _settingsManager.FilteredFileFormat);

            IgnorableFileFormat = string.Join(SettingsManager.SEPARATOR.ToString(), _settingsManager.IgnorableFileFormat);

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
                    FolderForHistory = _getFolderManager.OpenFolderPath(FolderForHistory);
                    return;
            }
        }

        private void Save()
        {
            _settingsManager.FolderForHistory = FolderForHistory;
            _settingsManager.FilteredFileFormat = FilteredFileFormat.Split(SettingsManager.SEPARATOR);
            _settingsManager.IgnorableFileFormat = IgnorableFileFormat.Split(SettingsManager.SEPARATOR);
            _settingsManager.IsUseFillter = IsUseFillter;
            _settingsManager.IsUseIgnoreFillter = IsUseIgnoreFillter;
        }

 

        #endregion Methods
    }
}

