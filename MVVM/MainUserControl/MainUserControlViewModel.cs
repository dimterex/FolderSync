namespace FolderSyns.MVVM.MainUserControl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FolderSyns.Annotations;
    using FolderSyns.Code;
    using FolderSyns.Code.Helpers;
    using FolderSyns.MVVM.HistoryUserControl;
    using FolderSyns.MVVM.SettingsUserControl;

    public class MainUserControlViewModel : INotifyPropertyChanged
    {
        #region Fields
        private readonly List<FileAction> _serializeObjects;

        private RelayCommand _refreshCommand;
        private RelayCommand _settingCommand;
        private RelayCommand _historyCommand;
        private RelayCommand _startCommand;
        private RelayCommand _closeCommand;

        private RelayCommand _openFolderCommand;

        private string _sourcePath;
        private string _targetPath;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Fields

        #region Properties
        public static MainUserControlViewModel Instance { get; } = new MainUserControlViewModel();

        public RelayCommand RefreshCommand => _refreshCommand;
        public RelayCommand SettingCommand => _settingCommand;
        public RelayCommand HistoryCommand => _historyCommand;
        public RelayCommand StartCommand => _startCommand;
        public RelayCommand CloseCommand => _closeCommand;

        /// <summary>
        /// Указать папку - источник ресурсов.
        /// </summary>
        public RelayCommand OpenFolderCommand => _openFolderCommand;

        public string SourcePath
        {
            get => _sourcePath;
            set
            {
                _sourcePath = value;
                OnPropertyChanged(nameof(SourcePath));
            }
        }

        public string TargetPath
        {
            get => _targetPath;
            set
            {
                _targetPath = value;
                OnPropertyChanged(nameof(TargetPath));
            }
        }

        public ObservableCollection<FileAction> InSourceFileNotExistTarget { get; }
        public ObservableCollection<FileAction> InTargetFileNotExistSource { get; }
        #endregion Properties

      



        private MainUserControlViewModel()
        {
            InSourceFileNotExistTarget = new ObservableCollection<FileAction>();
            InTargetFileNotExistSource = new ObservableCollection<FileAction>();

            _serializeObjects = HistoryModel.Inctance.DeSerializeObject<List<FileAction>>() ?? new List<FileAction>();
            InitComands();
            SourcePath = SettingsModel.Inctance.DefaultSourceFolder;
            TargetPath = SettingsModel.Inctance.DefaultTargetFolder;
        }

        #region Methods

        private void InitComands()
        {
            _refreshCommand = new RelayCommand(RefreshAction);
            _settingCommand = new RelayCommand(SettingsAction);
            _historyCommand = new RelayCommand(HistoryAction);

            _startCommand = new RelayCommand(StartAction);
            _closeCommand = new RelayCommand(CloseAction);

            _openFolderCommand = new RelayCommand(OpenFolderPath);
        }

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
                    SourcePath = OpenFolderDialog.OpenFolderPath(SourcePath);
                    return;
                case FolderType.TargetFolder:
                    TargetPath = OpenFolderDialog.OpenFolderPath(TargetPath);
                    return;
            }
        }

        /// <summary>
        /// Загзурить файлы из папки и подпапок.
        /// </summary>
        private List<string> ReadLogFile(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                return new List<string>();

            var folders = Directory.GetDirectories(folderPath);
            var files = Directory.GetFiles(folderPath).ToList();
            foreach (var folder in folders)
                files.AddRange(ReadLogFile(folder));
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

            var sourceFiles = ReadLogFile(SourcePath);
            var targetFiles = ReadLogFile(TargetPath);

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
        private void StartAction()
        {
            StartActions(TargetPath, InSourceFileNotExistTarget);
            StartActions(SourcePath, InTargetFileNotExistSource);

            _serializeObjects.AddRange(InSourceFileNotExistTarget.Where(f => f.IsCopy || f.IsDelete));
            _serializeObjects.AddRange(InTargetFileNotExistSource.Where(f => f.IsCopy || f.IsDelete));

            MessageBox.Show("Выполнено");

            RefreshAction();
        }

        /// <summary>
        /// Выполнить действия.
        /// </summary>
        private void StartActions(string folder, ObservableCollection<FileAction> logInfos)
        {
            foreach (var item in logInfos)
            {
                if (item.IsCopy)
                    File.Copy(item.OldFolder + item.FileName, folder + item.FileName);
                if (item.IsDelete)
                    File.Delete(item.OldFolder + item.FileName);

                item.DateTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Закрытие приложения. Сохранения истории.
        /// </summary>
        private void CloseAction()
        { 
            SettingsModel.Inctance.SetDefaultSourceFolder(SourcePath);
            SettingsModel.Inctance.SetDefaultTargetFolder(TargetPath);
            HistoryModel.Inctance.SerializeObject(_serializeObjects);
            // PushNewCommit();
        }

        private void PushNewCommit()
        {
            string cdCommand = @"cd D:\Синхронизация\Музыка";
            string gitCommand = "git";
            string gitAddArgument = @"pause";
            string gitCommitArgument = @"commit ""explanations_of_changes"" ";
            string gitPushArgument = @"push our_remote";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                },
            };
            process.Start();

            using (var writer = process.StandardInput)
            {
                writer.WriteLine();
                process.WaitForExit();
                writer.Close();
            }
        }

        /// <summary>
        /// Открыть окно истории изменений.
        /// </summary>
        private void HistoryAction()
        {
            HistoryViewModel.Instance.SetHistory(_serializeObjects);
            var historyView = new HistoryView
            {
                DataContext = HistoryViewModel.Instance
            };
            historyView.Show(); 
        }

        private void SettingsAction()
        {
            var settingsView = new SettingsView
            {
                DataContext = SettingsViewModel.Inctance
            };
            settingsView.Show();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion Methods
    }
}
