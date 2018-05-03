namespace FolderSyns.MVVM.HistoryUserControl
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FolderSyns.Annotations;
    using FolderSyns.Code;

    public class HistoryViewModel : INotifyPropertyChanged
    {
        #region Fields
        private List<FileAction> _fileActions;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Fields

        #region Properties
        public static HistoryViewModel Instance { get; } = new HistoryViewModel();

        public List<FileAction> FileActions
        {
            get => _fileActions;
            set
            {
                _fileActions = value;
                OnPropertyChanged(nameof(FileActions));
            }
        }
        #endregion Properties

        #region Methods
        public void SetHistory(List<FileAction> fileActions)
        {
            FileActions = fileActions;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion Methods
    }
}
