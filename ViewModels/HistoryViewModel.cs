namespace FolderSyns.ViewModels
{
    using FolderSyns.Code;
    using FolderSyns.Interfaces;

    using Prism.Mvvm;

    using System.Collections.Generic;

    public class HistoryViewModel : BindableBase
    {
        #region Fields

        private List<FileAction> _fileActions;

        #endregion Fields

        #region Properties

        public List<FileAction> FileActions
        {
            get => _fileActions;
            set => SetProperty(ref _fileActions, value);
        }

        #endregion Properties

        public HistoryViewModel(IHistoryManager historyManager)
        {
            FileActions = historyManager.DeSerializeObject<List<FileAction>>() ?? new List<FileAction>();
        }
    }
}
