namespace FolderSyns.Code
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    [Serializable]
    public class FileAction : INotifyPropertyChanged
    {
        [NonSerialized]
        private bool _isCopy;
        [NonSerialized]
        private bool _isDelete;
 
        public event PropertyChangedEventHandler PropertyChanged;

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
