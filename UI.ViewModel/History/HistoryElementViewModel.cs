using System;
using Core.Enum;
using Core.Model.History;

namespace UI.ViewModel.History
{
    public class HistoryElementViewModel
    {
        public HistoryElementViewModel(HistoryObjectModel historyObjectModel)
        {
            IsCopy = historyObjectModel.FileActions.HasFlag(FileActions.Copy);
            IsDelete = historyObjectModel.FileActions.HasFlag(FileActions.Delete);
            FileName = historyObjectModel.FileName;
            NewFolder = historyObjectModel.NewFolder;
            OldFolder = historyObjectModel.OldFolder;
            DateTime = historyObjectModel.DateTime;
        }

        public bool IsCopy { get; }

        public bool IsDelete { get; }

        public string FileName { get; }

        public string NewFolder { get; }

        public string OldFolder { get; }

        public DateTime DateTime { get; }
    }
}