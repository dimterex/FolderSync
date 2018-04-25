using FolderSyns.MVVM.HistoryUserControl;

namespace FolderSyns.MVVM.MainUserControl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using FolderSyns.Code;
    using FolderSyns.Code.Helpers;
    using FolderSyns.Windows;

    public class MainUserControlViewModel
    {
        private readonly List<FileAction> _serializeObjects;

        private RelayCommand _refreshCommand;
        private RelayCommand _settingCommand;
        private RelayCommand _historyCommand;
        private RelayCommand _startCommand;
        private RelayCommand _closeCommand;

        private RelayCommand _openFolderCommand;

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

        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public ObservableCollection<FileAction> InSourceFileNotExistTarget { get; set; }
        public ObservableCollection<FileAction> InTargetFileNotExistSource { get; set; }

        private MainUserControlViewModel()
        {
            InSourceFileNotExistTarget = new ObservableCollection<FileAction>();
            InTargetFileNotExistSource = new ObservableCollection<FileAction>();
            _serializeObjects = HistoryModel.Inctance.DeSerializeObject<List<FileAction>>() ?? new List<FileAction>();
            InitComands();
            SourcePath = @"D:\Синхронизация\Музыка";
        }

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
            if (!bool.TryParse(param.ToString(), out bool isSource))
                return;

            var folderBrowserDialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(SourcePath) && isSource)
                folderBrowserDialog.SelectedPath = SourcePath;
            else if (!isSource && !string.IsNullOrEmpty(TargetPath))
                folderBrowserDialog.SelectedPath = TargetPath;

            folderBrowserDialog.Description = isSource ? "Укажите первую папку" : "Укажите вторую папку";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                if (isSource)
                    SourcePath = folderBrowserDialog.SelectedPath;
                else
                    TargetPath = folderBrowserDialog.SelectedPath;
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
                OpenFolderPath(true);

            if (string.IsNullOrEmpty(TargetPath))
                OpenFolderPath(false);

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

            _serializeObjects.AddRange(InSourceFileNotExistTarget);
            _serializeObjects.AddRange(InTargetFileNotExistSource);

            System.Windows.Forms.MessageBox.Show("Выполнено");

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
            var historyView = new HistoryView();
            historyView.DataContext = HistoryViewModel.Instance;
            historyView.Show(); 
        }

        private void SettingsAction()
        {
            var settingsView = new SettingsUserControl.SettingsView();
            settingsView.DataContext = SettingsUserControl.SettingsViewModel.Inctance;
            settingsView.Show();
        }
    }
}
