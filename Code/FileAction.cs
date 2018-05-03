namespace FolderSyns.Code
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FolderSyns.Annotations;

    [Serializable]
    public class FileAction : INotifyPropertyChanged
    {
        #region Fields
        [NonSerialized]
        private bool _isCopy;
        [NonSerialized]
        private bool _isDelete;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Fields

        #region Properties
        public bool IsCopy
        {
            get => _isCopy;
            set
            {
                _isCopy = value;
                OnPropertyChanged(nameof(IsCopy));
            }
        }

        public bool IsDelete
        {
            get => _isDelete;
            set
            {
                _isDelete = value;
                OnPropertyChanged(nameof(IsDelete));
            }
        }

        public string FileName { get; set; }

        public string NewFolder { get; set; }

        public string OldFolder { get; set; }

        public DateTime DateTime { get; set; }
        #endregion Properties

        public FileAction()
        {
        }

        public FileAction(string fileName, string oldFolder, string newFolder)
        {
            OldFolder = oldFolder;
            FileName = fileName.Replace(oldFolder, string.Empty);
            NewFolder = newFolder;
            IsCopy = false;
            IsDelete = false;
        }

        #region Methods
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion Methods
    }
}
