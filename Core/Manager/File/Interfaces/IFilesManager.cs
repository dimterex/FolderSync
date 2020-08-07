using System.Collections.Generic;
using Core.Model.File;

namespace Core.Manager.File.Interfaces
{
    public interface IFilesManager
    {
        IList<FileElementModel> CompairFolders(string oldFolder, string newFolder);
        void StartActions(string sourcePath, string p1, IEnumerable<FileElementModel> select);
    }
}