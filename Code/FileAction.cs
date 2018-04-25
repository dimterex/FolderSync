namespace FolderSyns.Code
{
    using System;
    using FolderSyns.Code.Helpers;

    [Serializable]
    public class FileAction
    {
        [NonSerialized]
        private bool _isCopy;
        [NonSerialized]
        private bool _isDelete;

        public bool IsCopy
        {
            get => _isCopy;
            set => PropertyHelper.SetProperty(ref _isCopy, value, this, nameof(IsCopy));
        }

        public bool IsDelete
        {
            get => _isDelete;
            set => PropertyHelper.SetProperty(ref _isDelete, value, this, nameof(IsDelete));
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
    }
}
