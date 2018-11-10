namespace FolderSyns.Code
{

    using Prism.Mvvm;

    using System;

    [Serializable]
    public class FileAction : BindableBase
    {
        #region Fields

        [NonSerialized]
        private bool _isCopy;

        [NonSerialized]
        private bool _isDelete;

        #endregion Fields

        #region Properties

        public bool IsCopy
        {
            get => _isCopy;
            set => SetProperty(ref _isCopy, value);
        }

        public bool IsDelete
        {
            get => _isDelete;
            set => SetProperty(ref _isDelete, value);
        }

        public string FileName { get; set; }

        public string NewFolder { get; set; }

        public string OldFolder { get; set; }

        public DateTime DateTime { get; set; }

        #endregion Properties
       
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
