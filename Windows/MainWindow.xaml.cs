namespace FolderSyns.Windows
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;

    using FolderSyns.Code;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<FileAction> _inSourceFileNotExistTarget;
        private List<FileAction> _inTargetFileNotExistSource;

        private readonly List<FileAction> _serializeObjects;

        public MainWindow()
        {
            InitializeComponent();
            _inSourceFileNotExistTarget = new List<FileAction>();
            _inTargetFileNotExistSource = new List<FileAction>();
            SourcePath.Text = @"D:\Синхронизация\Музыка";

            Closed += OnClosed;
            _serializeObjects = History.DeSerializeObject<List<FileAction>>() ?? new List<FileAction>();
        }

        /// <summary>
        /// Указать папку - источник ресурсов.
        /// </summary>
        private void SourceOpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderPath(true);
        }

        /// <summary>
        /// Указать папку - получатель ресурсов.
        /// </summary>
        private void TargetOpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderPath(false);
        }

        /// <summary>
        /// Указать папку.
        /// </summary>
        /// <param name="isFirst">Указывается первая папка.</param>
        private void OpenFolderPath(bool isFirst)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(SourcePath.Text) && isFirst)
                folderBrowserDialog.SelectedPath = SourcePath.Text;
            else if (!isFirst && !string.IsNullOrEmpty(TargetPath.Text))
                folderBrowserDialog.SelectedPath = TargetPath.Text;

            folderBrowserDialog.Description = isFirst ? "Укажите первую папку" : "Укажите вторую папку";
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (isFirst)
                    SourcePath.Text = folderBrowserDialog.SelectedPath;
                else
                    TargetPath.Text = folderBrowserDialog.SelectedPath;
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
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SourcePath.Text))
                OpenFolderPath(true);

            if (string.IsNullOrEmpty(TargetPath.Text))
                OpenFolderPath(false);

            var sourceFiles = ReadLogFile(SourcePath.Text);
            var targetFiles = ReadLogFile(TargetPath.Text);

            _inSourceFileNotExistTarget = CompairFolders(sourceFiles, targetFiles, SourcePath.Text, TargetPath.Text);
            _inTargetFileNotExistSource = CompairFolders(targetFiles, sourceFiles, TargetPath.Text, SourcePath.Text);

            SourceFileNotExistTarget.ItemsSource = _inSourceFileNotExistTarget;
            TargetFileNotExistSource.ItemsSource = _inTargetFileNotExistSource;
        }

        /// <summary>
        /// Сравнение списков файлов.
        /// </summary>
        /// <returns></returns>
        private List<FileAction> CompairFolders(List<string> sourceFiles, List<string> targetFiles, string oldFolder, string newFolder)
        {
            var sourceFilesNotTarget = new List<FileAction>();
            foreach (var sourceFile in sourceFiles)
            {
                var s = sourceFile.Replace(oldFolder, string.Empty);
                if (!targetFiles.Any(x => x.EndsWith(s)))
                    sourceFilesNotTarget.Add(new FileAction(sourceFile, oldFolder, newFolder));
            }

            return sourceFilesNotTarget;
        }

        /// <summary>
        /// Закрыть программу.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Выполнить действия.
        /// </summary>
        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            StartActions(TargetPath.Text, _inSourceFileNotExistTarget);
            StartActions(SourcePath.Text, _inTargetFileNotExistSource);

            _serializeObjects.AddRange(_inSourceFileNotExistTarget);
            _serializeObjects.AddRange(_inTargetFileNotExistSource);

            System.Windows.Forms.MessageBox.Show("Выполнено");

            RefreshButton_Click(null, new RoutedEventArgs());
        }

        /// <summary>
        /// Выполнить действия.
        /// </summary>
        private void StartActions(string folder, List<FileAction> logInfos)
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
        private void OnClosed(object sender, EventArgs eventArgs)
        {
            History.SerializeObject(_serializeObjects);
        }

        /// <summary>
        /// Открыть окно истории изменений.
        /// </summary>
        private void HistoryButton_OnClick(object sender, RoutedEventArgs e)
        {
           new HistoryWindow(_serializeObjects).Show();
        }
    }
}
