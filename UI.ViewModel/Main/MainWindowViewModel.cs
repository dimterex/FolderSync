using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Core;
using Core.Manager.Dialog;
using Core.Manager.Event;
using Core.Manager.Event.Interfaces;
using Core.Manager.File.Interfaces;
using Core.Manager.Settings.Interface;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using UI.ViewModel.FolderInfo;
using UI.ViewModel.Locale;

namespace UI.ViewModel.Main
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel(ISettingsManager settingsManager,
            IFolderDialogManager folderDialogManager,
            IFilesManager filesManager,
            IEventManager eventManager)
        {
            _settingsManager = settingsManager;
            _filesManager = filesManager;
            _eventManager = eventManager;
            LocalizationManager.Instance.SetCulture(_settingsManager.SettingsModel.CultureInfo);


            _eventManager.CopyActionStarted += FilesManagerOnCopyActionStarted;
            _eventManager.RemoveActionStarted += FilesManagerOnRemoveActionStarted;
            _eventManager.ActionCompleted += FilesManagerOnActionCompleted;

            SourceViewModel = new FolderInfoViewModel(FolderType.SourceFolder, _settingsManager, folderDialogManager,
                filesManager);
            TargetViewModel = new FolderInfoViewModel(FolderType.TargetFolder, _settingsManager, folderDialogManager,
                filesManager);

            HistoryCommandNotificationRequest = new InteractionRequest<INotification>();
            SettingsCommandNotificationRequest = new InteractionRequest<INotification>();
            CompletedNotificationRequest = new InteractionRequest<INotification>();

            RefreshCommand = new DelegateCommand(RefreshAction);
            SettingCommand = new DelegateCommand(SettingsAction);
            HistoryCommand = new DelegateCommand(HistoryAction);

            StartCommand = new DelegateCommand(StartAction);
            CloseCommand = new DelegateCommand(CloseAction);
            
            FileAction = FileAction.Not;
        }

        private void FilesManagerOnActionCompleted(object sender, EventModel e)
        {
            CompletedCount++;
        }

        private void FilesManagerOnRemoveActionStarted(object sender, string e)
        {
            FileAction = FileAction.Removing;
            CurrentFile = e;
        }

        public FileAction FileAction
        {
            get => _fileAction;
            set => SetProperty(ref _fileAction, value);
        }

        private void FilesManagerOnCopyActionStarted(object sender, string e)
        {
            FileAction = FileAction.Copying;
            CurrentFile = e;
        }

        #region Fields

        private readonly ISettingsManager _settingsManager;
        private readonly IFilesManager _filesManager;
        private readonly IEventManager _eventManager;
        private string _currentFile;

        private int _completedCount;
        private int _selectedCount;
        private FileAction _fileAction;

        #endregion Fields

        #region Properties

        public ICommand RefreshCommand { get; }

        public ICommand SettingCommand { get; }

        public ICommand HistoryCommand { get; }

        public ICommand StartCommand { get; }

        public ICommand CloseCommand { get; }

        public InteractionRequest<INotification> HistoryCommandNotificationRequest { get; }
        public InteractionRequest<INotification> SettingsCommandNotificationRequest { get; }

        public InteractionRequest<INotification> CompletedNotificationRequest { get; }

        public string CurrentFile
        {
            get => _currentFile;
            set => SetProperty(ref _currentFile, value);
        }

        public int CompletedCount
        {
            get => _completedCount;
            set => SetProperty(ref _completedCount, value);
        }

        public int SelectedCount
        {
            get => _selectedCount;
            set => SetProperty(ref _selectedCount, value);
        }

        public FolderInfoViewModel SourceViewModel { get; }
        public FolderInfoViewModel TargetViewModel { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Пересканировать папки.
        /// </summary>
        private void RefreshAction()
        {
            SourceViewModel.InSourceFileNotExistTarget.Clear();
            TargetViewModel.InSourceFileNotExistTarget.Clear();

            if (string.IsNullOrEmpty(SourceViewModel.SourcePath))
                SourceViewModel.OpenFolderCommand.Execute(null);

            if (string.IsNullOrEmpty(TargetViewModel.SourcePath))
                TargetViewModel.OpenFolderCommand.Execute(null);

            var sources = _filesManager.CompairFolders(SourceViewModel.SourcePath, TargetViewModel.SourcePath);
            var targets = _filesManager.CompairFolders(TargetViewModel.SourcePath, SourceViewModel.SourcePath);

            foreach (var fileElementModel in sources)
                SourceViewModel.InSourceFileNotExistTarget.Add(new FileElementViewModel(fileElementModel));

            foreach (var fileElementModel in targets)
                TargetViewModel.InSourceFileNotExistTarget.Add(new FileElementViewModel(fileElementModel));
            
            FileAction = FileAction.Not;
            CurrentFile = string.Empty;
        }

        /// <summary>
        ///     Выполнить действия.
        /// </summary>
        private async void StartAction()
        {
            CompletedCount = 0;

            var targets = TargetViewModel.InSourceFileNotExistTarget.Where(x => x.IsCopy || x.IsDelete)
                .Select(x => x.GetModel()).ToList();

            var sources = SourceViewModel.InSourceFileNotExistTarget.Where(x => x.IsCopy || x.IsDelete)
                .Select(x => x.GetModel()).ToList();

            SelectedCount = targets.Count + sources.Count;

            var result = Task.Factory.StartNew(() =>
            {
                _filesManager.StartActions(TargetViewModel.SourcePath, SourceViewModel.SourcePath, targets);
                _filesManager.StartActions(SourceViewModel.SourcePath, TargetViewModel.SourcePath, sources);
            });
            await result;

            CompletedNotificationRequest?.Raise(new Notification());

            RefreshAction();
        }

        /// <summary>
        ///     Закрытие приложения. Сохранения истории.
        /// </summary>
        private void CloseAction()
        {
            _settingsManager.SaveSettings();
        }

        /// <summary>
        ///     Открыть окно истории изменений.
        /// </summary>
        private void HistoryAction()
        {
            HistoryCommandNotificationRequest?.Raise(new Notification());
        }

        private void SettingsAction()
        {
            SettingsCommandNotificationRequest?.Raise(new Notification());
        }

        #endregion Methods
    }
}