using System;
using Core.Enum;

namespace Core.Model.History
{
    public class HistoryObjectModel
    {
        public HistoryObjectModel(string fileName, string oldFolder, string newFolder)
        {
            OldFolder = oldFolder;
            FileName = fileName.Replace(oldFolder, string.Empty);
            NewFolder = newFolder;
            FileActions = FileActions.Not;
        }

        #region Properties

        public FileActions FileActions { get; set; }

        public string FileName { get; set; }

        public string NewFolder { get; set; }

        public string OldFolder { get; set; }

        public DateTime DateTime { get; set; }

        #endregion Properties
    }
}