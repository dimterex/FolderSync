using Core.Enum;
using Core.Extensions;
using Core.Model.File;
using Prism.Mvvm;

namespace UI.ViewModel.FolderInfo
{
    public class FileElementViewModel : BindableBase
    {
        private readonly FileElementModel _fileElementModel;
        private bool _isCopy;
        private bool _isDelete;

        public FileElementViewModel(FileElementModel fileElementModel)
        {
            _fileElementModel = fileElementModel;
            IsCopy = _fileElementModel.FileActions.HasFlag(FileActions.Copy);
            IsDelete = _fileElementModel.FileActions.HasFlag(FileActions.Delete);
            FileName = _fileElementModel.Path;
        }

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

        public FileElementModel GetModel()
        {
            if (IsCopy)
                _fileElementModel.AddFlags(FileActions.Copy);
            else
                _fileElementModel.RemoveFlags(FileActions.Copy);
            
            if (IsDelete)
                _fileElementModel.AddFlags(FileActions.Delete);
            else
                _fileElementModel.RemoveFlags(FileActions.Delete);
            
            return _fileElementModel;
        }
    }
}