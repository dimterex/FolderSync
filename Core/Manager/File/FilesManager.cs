using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Enum;
using Core.Manager.Event;
using Core.Manager.Event.Interfaces;
using Core.Manager.File.Interfaces;
using Core.Manager.Settings.Interface;
using Core.Model.File;

namespace Core.Manager.File
{
    public class FilesManager : IFilesManager
    {
        private readonly IList<string> _audioFilesFormat;
        private readonly IEventManager _eventManager;
        private readonly IList<string> _ignoreFilesFormat;
        private readonly bool _isUseFillter;
        private readonly bool _isUseIgnoreFillter;
        private readonly ISettingsManager _settingsManager;

        public FilesManager(ISettingsManager settingsManager, IEventManager eventManager)
        {
            _settingsManager = settingsManager;
            _eventManager = eventManager;
            _isUseFillter = _settingsManager.SettingsModel.IsUseFilter;
            _isUseIgnoreFillter = _settingsManager.SettingsModel.IsUseIgnoreFilter;

            _audioFilesFormat = _settingsManager.SettingsModel.FilteredFileFormat;
            _ignoreFilesFormat = _settingsManager.SettingsModel.IgnorableFileFormat;
        }

        /// <summary>
        ///     Сравнение списков файлов.
        /// </summary>
        public IList<FileElementModel> CompairFolders(string oldFolder, string newFolder)
        {
            var sourceFiles = GetFileList(oldFolder);
            var targetFiles = GetFileList(newFolder);
            var result = new List<FileElementModel>();
            foreach (var sourceFile in sourceFiles)
            {
                var s = sourceFile.Path.Replace(oldFolder, string.Empty);
                if (!targetFiles.Any(x => x.Path.EndsWith(s)))
                {
                    var fileElementModel = new FileElementModel();
                    fileElementModel.Path = s;
                    result.Add(fileElementModel);
                }
            }

            return result;
        }

        /// <summary>
        ///     Выполнить действия.
        /// </summary>
        public void StartActions(string from, string to, IEnumerable<FileElementModel> logInfos)
        {
            foreach (var item in logInfos)
            {
                if (item.FileActions.HasFlag(FileActions.Copy))
                {
                    _eventManager.RaiseStartCopyAction(item.Path);
                    CopyFile(from, to, item.Path);
                }

                if (item.FileActions.HasFlag(FileActions.Delete))
                {
                    _eventManager.RaiseStartRemoveAction(item.Path);
                    var filepath = $@"{from + item.Path}";
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                    if (Directory.GetFiles(from).Length == 0 && Directory.GetDirectories(from).Length == 0)
                        Directory.Delete(from);
                }

                var eventModel = new EventModel(item.Path, from, to, item.FileActions);

                _eventManager.RaiseActionCompleted(eventModel);
            }
        }

        /// <summary>
        ///     Загзурить файлы из папки и подпапок.
        /// </summary>
        private List<FileElementModel> GetFileList(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                return new List<FileElementModel>();

            bool IsFilltered(string fileName)
            {
                var result = true;

                if (_isUseFillter)
                    result = _audioFilesFormat.Any(fileName.EndsWith);

                if (_isUseIgnoreFillter)
                    result |= _ignoreFilesFormat.Any(fileName.EndsWith);

                return result;
            }

            var folders = Directory.GetDirectories(folderPath);
            var files = Directory.GetFiles(folderPath).Where(IsFilltered).ToList();
            var ls = new List<FileElementModel>();

            foreach (var file in files)
            {
                var item = new FileElementModel();
                item.Path = file;
                ls.Add(item);
            }

            foreach (var folder in folders)
                ls.AddRange(GetFileList(folder));
            return ls;
        }

        private void CopyFile(string oldFolder, string newFolder, string fileName)
        {
            var name = fileName.Split(Path.DirectorySeparatorChar);

            var path = string.Empty;
            for (var i = 0; i < name.Length - 1; i++)
            {
                path += name[i] + Path.DirectorySeparatorChar;
                if (!Directory.Exists(newFolder + path))
                    Directory.CreateDirectory(newFolder + path);
            }

            if (!System.IO.File.Exists(newFolder + fileName))
                System.IO.File.Copy(oldFolder + fileName, newFolder + fileName);
        }
    }
}