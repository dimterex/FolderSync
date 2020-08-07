using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Core;
using Core.Manager.Dialog;
using Core.Manager.File.Interfaces;
using Core.Manager.Settings.Interface;
using Prism.Commands;
using Prism.Mvvm;

namespace UI.ViewModel.FolderInfo
{
    public class FolderInfoViewModel : BindableBase
    {
        public FolderInfoViewModel(FolderType folderType, ISettingsManager settingsManager,
            IFolderDialogManager folderDialogManager, IFilesManager filesManager)
        {
            _folderType = folderType;
            _settingsManager = settingsManager;
            _folderDialogManager = folderDialogManager;
            _filesManager = filesManager;
            InSourceFileNotExistTarget = new ObservableCollection<FileElementViewModel>();
            OpenFolderCommand = new DelegateCommand(OpenFolderPath);

            switch (_folderType)
            {
                case FolderType.SourceFolder:
                    SourcePath = _settingsManager.SettingsModel.DefaultSourceFolder;
                    break;
                case FolderType.TargetFolder:
                    SourcePath = _settingsManager.SettingsModel.DefaultTargetFolder;
                    break;
                case FolderType.FolderForHistory:
                    SourcePath = _settingsManager.SettingsModel.FolderForHistory;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_folderType), _folderType, null);
            }
        }

        #region Methods

        /// <summary>
        ///     Указать папку.
        /// </summary>
        private void OpenFolderPath()
        {
            SourcePath = _folderDialogManager.OpenFolderPath(SourcePath);

            switch (_folderType)
            {
                case FolderType.SourceFolder:
                    _settingsManager.SettingsModel.DefaultSourceFolder = SourcePath;
                    break;
                case FolderType.TargetFolder:
                    _settingsManager.SettingsModel.DefaultTargetFolder = SourcePath;
                    break;
                case FolderType.FolderForHistory:
                    _settingsManager.SettingsModel.FolderForHistory = SourcePath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_folderType), _folderType, null);
            }
        }

        #endregion Methods

        #region Fields

        private string _sourcePath;
        private bool _isAllCopySource;
        private bool _isAllDeleteSource;

        private readonly FolderType _folderType;
        private readonly ISettingsManager _settingsManager;
        private readonly IFolderDialogManager _folderDialogManager;
        private readonly IFilesManager _filesManager;

        #endregion Fields

        #region Properties

        /// <summary>
        ///     Указать папку - источник ресурсов.
        /// </summary>
        public ICommand OpenFolderCommand { get; }

        public string SourcePath
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }

        public ObservableCollection<FileElementViewModel> InSourceFileNotExistTarget { get; }

        public bool IsAllCopySource
        {
            get => _isAllCopySource;
            set
            {
                if (SetProperty(ref _isAllCopySource, value))
                    foreach (var file in InSourceFileNotExistTarget)
                        file.IsCopy = value;
            }
        }

        public bool IsAllDeleteSource
        {
            get => _isAllDeleteSource;
            set
            {
                if (SetProperty(ref _isAllDeleteSource, value))
                    foreach (var file in InSourceFileNotExistTarget)
                        file.IsDelete = value;
            }
        }

        #endregion Properties
    }
}