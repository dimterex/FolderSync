namespace FolderSyns.Core
{
    using Prism.Mvvm;

    using System.Collections.Generic;

    using FolderSyns.Interfaces;

    public class SettingsManager : BindableBase, ISettingsManager
    {
        public const char SEPARATOR = ';';

        private const string FOLDER_FOR_HISTORY = "FolderForHistory";
        private const string DEFAULT_SOURCE_FOLDER = "DefaultSourceFolder";
        private const string DEFAULT_TARGET_FOLDER = "DefaultTargetFolder";
        private const string USE_FILLTER = "IsUseFillter";
        private const string USE_IGNOREFILLTER = "IsUseIgnoreFillter";
        private const string IGNORABLE_ARRAY = "IgnorableFileFormat";
        private const string FILLTER_ARRAY = "FilteredFileFormat";
        
        private readonly IConfigManager _configManager;

        private string _folderForHistory;
        private string _defaultSourceFolder;
        private string _defaultTargetFolder;

        private bool _isUseFillter;
        private bool _isUseIgnoreFillter;
        private IList<string> _ignorableFileFormat;
        private IList<string> _filteredFileFormat;

        #region Properties

        public string FolderForHistory
        {
            get => _folderForHistory;
            set
            {
                if (SetProperty(ref _folderForHistory, value))
                    _configManager.SaveFolderForHistory(FOLDER_FOR_HISTORY, value);
            }
        }

        public bool IsUseFillter
        {
            get => _isUseFillter;
            set
            {
                if (SetProperty(ref _isUseFillter, value))
                    _configManager.SaveFolderForHistory(USE_FILLTER, value);
            }
        }

        public bool IsUseIgnoreFillter
        {
            get => _isUseIgnoreFillter;
            set
            {
                if (SetProperty(ref _isUseIgnoreFillter, value))
                    _configManager.SaveFolderForHistory(USE_IGNOREFILLTER, value);
            }
        }

        public string DefaultSourceFolder
        {
            get => _defaultSourceFolder;
            set
            {
                if(SetProperty( ref _defaultSourceFolder, value))
                    _configManager.SaveFolderForHistory(DEFAULT_SOURCE_FOLDER, value);
            }
        }

        public string DefaultTargetFolder
        {
            get => _defaultTargetFolder;
            set
            {
                if (SetProperty(ref _defaultTargetFolder, value))
                    _configManager.SaveFolderForHistory(DEFAULT_TARGET_FOLDER, value);
            }
        }

        public IList<string> IgnorableFileFormat
        {
            get => _ignorableFileFormat;
            set
            {
                if (SetProperty(ref _ignorableFileFormat, value))
                    _configManager.SaveFolderForHistory(IGNORABLE_ARRAY, string.Join(SEPARATOR.ToString(), value));
            }
        }

        public IList<string> FilteredFileFormat
        {
            get => _filteredFileFormat;
            set
            {
                if (SetProperty(ref _filteredFileFormat, value))
                    _configManager.SaveFolderForHistory(FILLTER_ARRAY, string.Join(SEPARATOR.ToString(), value));
            }
        }

        #endregion Properties

        public SettingsManager(IConfigManager configManager)
        {
            _configManager = configManager;

            FolderForHistory = _configManager.LoadSetting<string>(FOLDER_FOR_HISTORY) ?? string.Empty;
            DefaultSourceFolder = _configManager.LoadSetting<string>(DEFAULT_SOURCE_FOLDER) ?? string.Empty;
            DefaultTargetFolder = _configManager.LoadSetting<string>(DEFAULT_TARGET_FOLDER) ?? string.Empty;
            IsUseFillter = _configManager.LoadSetting<bool>(USE_FILLTER);
            IsUseIgnoreFillter = _configManager.LoadSetting<bool>(USE_IGNOREFILLTER);
            var ignorString = _configManager.LoadSetting<string>(IGNORABLE_ARRAY) ?? string.Empty;
           
            if (!string.IsNullOrEmpty(ignorString))
                IgnorableFileFormat = new List<string>(ignorString.Split(SEPARATOR));
            else
                IgnorableFileFormat = new List<string>();

            var filterString = _configManager.LoadSetting<string>(FILLTER_ARRAY) ?? string.Empty;
            if (!string.IsNullOrEmpty(filterString))
                FilteredFileFormat = new List<string>(filterString.Split(SEPARATOR));
            else
                FilteredFileFormat = new List<string>();
        }

     
    }
}
