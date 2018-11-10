namespace FolderSyns.ViewModels
{
    using FolderSyns.Code;
    using FolderSyns.Enums;
    using FolderSyns.Interfaces;

    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public class MainUserControlViewModel : BindableBase
    {
        #region Fields

        private string _sourcePath;
        private string _targetPath;
        private bool _isAllCopySource;
        private bool _isAllDeleteSource;
        private bool _isAllCopyTarget;
        private bool _isAllDeleteTarget;

        private readonly bool _isUseFillter;
        private readonly bool _isUseIgnoreFillter;

        private readonly IList<string> _audioFilesFormat;
        private readonly IList<string> _ignoreFilesFormat;
        private readonly ISettingsManager _settingsManager;
        private readonly IHistoryManager _historyManager;
        private readonly IGetFolderManager _getFolderManager;
        private string _currentLog;
  
        private int _currentId;

        #endregion Fields

        #region Properties

        public RelayCommand RefreshCommand { get; }

        public RelayCommand SettingCommand { get; }

        public RelayCommand HistoryCommand { get; }

        public RelayCommand StartCommand { get; }

        public RelayCommand CloseCommand { get; }

        public InteractionRequest<INotification> HistoryCommandNotificationRequest { get; }
        public InteractionRequest<INotification> SettingsCommandNotificationRequest { get; }

        /// <summary>
        /// Указать папку - источник ресурсов.
        /// </summary>
        public RelayCommand OpenFolderCommand { get; }

        public string SourcePath
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }

        public string TargetPath
        {
            get => _targetPath;
            set => SetProperty(ref _targetPath, value);
        }

        public ObservableCollection<FileAction> InSourceFileNotExistTarget { get; }
        public ObservableCollection<FileAction> InTargetFileNotExistSource { get; }

        public bool IsAllCopySource
        {
            get => _isAllCopySource;
            set
            {
                if (SetProperty(ref _isAllCopySource, value))
                {
                    foreach (var file in InSourceFileNotExistTarget)
                        file.IsCopy = value;
                }
            }
        }

        public bool IsAllDeleteSource
        {
            get => _isAllDeleteSource;
            set
            {
                if (SetProperty(ref _isAllDeleteSource, value))
                {
                    foreach (var file in InSourceFileNotExistTarget)
                        file.IsDelete = value;
                }
            }
        }

        public bool IsAllCopyTarget
        {
            get => _isAllCopyTarget;
            set
            {
                if (SetProperty(ref _isAllCopyTarget, value))
                {
                    foreach (var file in InTargetFileNotExistSource)
                        file.IsCopy = value;
                }
            }
        }

        public bool IsAllDeleteTarget
        {
            get => _isAllDeleteTarget;
            set
            {
                if (SetProperty(ref _isAllDeleteTarget, value))
                {
                    foreach (var file in InTargetFileNotExistSource)
                        file.IsDelete = value;
                }
            }
        }

        public string CurrentLog
        {
            get => _currentLog;
            set => SetProperty(ref _currentLog, value);
        }

        public string Process => $"{_currentId}/{GetSelectedItem(InSourceFileNotExistTarget).Count() + GetSelectedItem(InTargetFileNotExistSource).Count()}";

        #endregion Properties

        public MainUserControlViewModel(ISettingsManager settingsManager, IHistoryManager historyManager, IGetFolderManager getFolderManager)
        {
            _settingsManager = settingsManager;
            _historyManager = historyManager;
            _getFolderManager = getFolderManager;

            HistoryCommandNotificationRequest = new InteractionRequest<INotification>();
            SettingsCommandNotificationRequest = new InteractionRequest<INotification>();

            InSourceFileNotExistTarget = new ObservableCollection<FileAction>();
            InTargetFileNotExistSource = new ObservableCollection<FileAction>();

            RefreshCommand = new RelayCommand(RefreshAction);
            SettingCommand = new RelayCommand(SettingsAction);
            HistoryCommand = new RelayCommand(HistoryAction);

            StartCommand = new RelayCommand(StartAction);
            CloseCommand = new RelayCommand(CloseAction);

            OpenFolderCommand = new RelayCommand(OpenFolderPath);

            SourcePath = _settingsManager.DefaultSourceFolder;
            TargetPath = _settingsManager.DefaultTargetFolder;

            _isUseFillter = _settingsManager.IsUseFillter;
            _isUseIgnoreFillter = _settingsManager.IsUseIgnoreFillter;

            _audioFilesFormat = _settingsManager.FilteredFileFormat;
            //_audioFilesFormat = new List<string>
            //{
            //    ".mp3",
            //    ".wav",
            //    ".flac",
            //    ".ape",
            //};

            _ignoreFilesFormat = _settingsManager.IgnorableFileFormat;
            //_ignoreFilesFormat = new List<string>
            //{
            //    ".ini",
            //    ".jpg",
            //    ".png",
            //    ".reapeaks",
            //};
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
                case FolderType.SourceFolder:
                    SourcePath = _getFolderManager.OpenFolderPath(SourcePath);
                    return;
                case FolderType.TargetFolder:
                    TargetPath = _getFolderManager.OpenFolderPath(TargetPath);
                    return;
            }
        }

        /// <summary>
        /// Загзурить файлы из папки и подпапок.
        /// </summary>
        private List<string> GetFileList(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                return new List<string>();

            bool IsFilltered(string fileName)
            {
                bool result = true;

                if (_isUseFillter)
                    result = _audioFilesFormat.Any(fileName.EndsWith);

                if (_isUseIgnoreFillter)
                    result |= _ignoreFilesFormat.Any(fileName.EndsWith);

                return result;
            }

            var folders = Directory.GetDirectories(folderPath);
            var files = Directory.GetFiles(folderPath).Where(IsFilltered).ToList();
            foreach (var folder in folders)
                files.AddRange(GetFileList(folder));
            return files;
        }

        /// <summary>
        /// Пересканировать папки.
        /// </summary>
        private void RefreshAction()
        {
            if (string.IsNullOrEmpty(SourcePath))
                OpenFolderPath(FolderType.SourceFolder);

            if (string.IsNullOrEmpty(TargetPath))
                OpenFolderPath(FolderType.TargetFolder);

            var sourceFiles = GetFileList(SourcePath);
            var targetFiles = GetFileList(TargetPath);

            CompairFolders(InSourceFileNotExistTarget, sourceFiles, targetFiles, SourcePath, TargetPath);
            CompairFolders(InTargetFileNotExistSource, targetFiles, sourceFiles, TargetPath, SourcePath);
        }

        /// <summary>
        /// Сравнение списков файлов.
        /// </summary>
        private void CompairFolders(ICollection<FileAction> observableCollection,
            IEnumerable<string> sourceFiles, IReadOnlyCollection<string> targetFiles,
            string oldFolder, string newFolder)
        {
            foreach (var sourceFile in sourceFiles)
            {
                var s = sourceFile.Replace(oldFolder, string.Empty);
                if (!targetFiles.Any(x => x.EndsWith(s)))
                    observableCollection.Add(new FileAction(sourceFile, oldFolder, newFolder));
            }
        }

        /// <summary>
        /// Выполнить действия.
        /// </summary>
        private async void StartAction()
        {
            _currentId = 0;
            RaisePropertyChanged(nameof(Process));

            var result = Task.Factory.StartNew(() =>
            {
                StartActions(TargetPath, InSourceFileNotExistTarget);
                StartActions(SourcePath, InTargetFileNotExistSource);

                _historyManager.SerializeObject(GetSelectedItem(InSourceFileNotExistTarget));
                _historyManager.SerializeObject(GetSelectedItem(InTargetFileNotExistSource));
            });
            await result;
           
            MessageBox.Show("Выполнено");

            InSourceFileNotExistTarget.Clear();
            InTargetFileNotExistSource.Clear();
            RefreshAction();
        }

        private IEnumerable<FileAction> GetSelectedItem(ICollection<FileAction> fileActions)
        {
            return fileActions.Where(f => f.IsCopy || f.IsDelete);
        }

        /// <summary>
        /// Выполнить действия.
        /// </summary>
        private void StartActions(string folder, IEnumerable<FileAction> logInfos)
        {
            foreach (var item in logInfos)
            {
                _currentId++;
                RaisePropertyChanged(nameof(Process));
                if (item.IsCopy)
                {
                    CurrentLog = $"Копируется файл: {item.FileName}.";
                    CopyFile(item.OldFolder, folder, item.FileName);
                }

                if (item.IsDelete)
                {
                    CurrentLog = $"Удаляется файл: {item.FileName}.";
                    File.Delete(item.OldFolder + item.FileName);
                    if (Directory.GetFiles(item.OldFolder).Length == 0)
                        Directory.Delete(item.OldFolder);
                }

                item.DateTime = DateTime.UtcNow;
            }
        }

        private void CopyFile(string oldFolder, string newFolder, string fileName)
        {
            string[] name = fileName.Split(Path.DirectorySeparatorChar);

            string path = string.Empty;
            for (int i = 0; i < name.Length - 1; i++)
            {
                path += name[i] + Path.DirectorySeparatorChar;
                if (!Directory.Exists(newFolder + path))
                    Directory.CreateDirectory(newFolder + path);
            }

            if (!File.Exists(newFolder + fileName))
                File.Copy(oldFolder + fileName, newFolder + fileName);
        }

        /// <summary>
        /// Закрытие приложения. Сохранения истории.
        /// </summary>
        private void CloseAction()
        {
            _settingsManager.DefaultSourceFolder = SourcePath;
            _settingsManager.DefaultTargetFolder = TargetPath;
        }

        /// <summary>
        /// Открыть окно истории изменений.
        /// </summary>
        private void HistoryAction()
        {
            HistoryCommandNotificationRequest?.Raise(new Notification()
            {
                Title = "История всего сделанного",
            });
        }

        private void SettingsAction()
        {
            SettingsCommandNotificationRequest?.Raise(new Notification()
            {
                Title = "Типа настроек, только круче",
            });
        }
      
        #endregion Methods
    }
}
