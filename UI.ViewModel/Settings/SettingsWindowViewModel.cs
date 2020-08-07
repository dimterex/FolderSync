using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Core.Manager.Dialog;
using Core.Manager.Settings.Interface;
using Prism.Commands;
using Prism.Mvvm;
using UI.ViewModel.Locale;
using UI.ViewModel.Locale.Interfaces;

namespace UI.ViewModel.Settings
{
    public class SettingsWindowViewModel : BindableBase
    {
        public SettingsWindowViewModel(ISettingsManager settingsManager, IFolderDialogManager folderDialogManager, ILocalizationManager localizationManager)
        {
            _settingsManager = settingsManager;
            _folderDialogManager = folderDialogManager;
            _localizationManager = localizationManager;

            FolderForHistory = settingsManager.SettingsModel.FolderForHistory;
            IsUseFilter = settingsManager.SettingsModel.IsUseFilter;
            IsUseIgnoreFilter = settingsManager.SettingsModel.IsUseIgnoreFilter;

            FilteredFileFormat = string.Join(";", settingsManager.SettingsModel.FilteredFileFormat);

            IgnorableFileFormat = string.Join(";", settingsManager.SettingsModel.IgnorableFileFormat);

            SaveCommand = new DelegateCommand(Save);
            OpenFolderCommand = new DelegateCommand(OpenFolderPath);

            Locale = _settingsManager.SettingsModel.CultureInfo.Name;
        }

        /// <summary>
        ///     Указать папку.
        /// </summary>
        private void OpenFolderPath()
        {
            FolderForHistory = _folderDialogManager.OpenFolderPath(FolderForHistory);
        }

        private void Save()
        {
            _settingsManager.SettingsModel.FolderForHistory = FolderForHistory;
            _settingsManager.SettingsModel.FilteredFileFormat = FilteredFileFormat.Split(';').ToList();
            _settingsManager.SettingsModel.IgnorableFileFormat = IgnorableFileFormat.Split(';').ToList();
            _settingsManager.SettingsModel.IsUseFilter = IsUseFilter;
            _settingsManager.SettingsModel.IsUseIgnoreFilter = IsUseIgnoreFilter;
            
            _settingsManager.SettingsModel.CultureInfo = new CultureInfo(Locale);
            _localizationManager.SetCulture(_settingsManager.SettingsModel.CultureInfo);
        }

        #region Fields

        private string _folderForHistory;

        private readonly IFolderDialogManager _folderDialogManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ISettingsManager _settingsManager;
        private string _filteredFileFormat;
        private string _ignorableFileFormat;
        private bool _isUseIgnoreFilter;
        private bool _isUseFilter;
        private string _locale;

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

        public bool IsUseFilter
        {
            get => _isUseFilter;
            set => SetProperty(ref _isUseFilter, value);
        }

        public bool IsUseIgnoreFilter
        {
            get => _isUseIgnoreFilter;
            set => SetProperty(ref _isUseIgnoreFilter, value);
        }

        public string Locale
        {
            get => _locale;
            set => SetProperty(ref _locale, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand OpenFolderCommand { get; }

        #endregion Properties
    }
}