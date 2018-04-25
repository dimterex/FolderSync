using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderSyns.Code;

namespace FolderSyns.MVVM.HistoryUserControl
{
    public class HistoryViewModel
    {
        public static HistoryViewModel Instance { get; } = new HistoryViewModel();

        private HistoryViewModel()
        {
        }

        public void SetHistory(List<FileAction> fileActions)
        {

        }
    }
}
