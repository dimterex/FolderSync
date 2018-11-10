namespace FolderSyns.Core
{
    using FolderSyns.Interfaces;

    using System.Windows.Forms;
    
    public class GetFolderManager : IGetFolderManager
    {
        /// <summary>
        /// Указать папку.
        /// </summary>
        /// <folderPath name="folderPath">Указывается первая папка.</folderPath>
        public string OpenFolderPath(string folderPath)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(folderPath))
                folderBrowserDialog.SelectedPath = folderPath;
  
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                return folderBrowserDialog.SelectedPath;

            return string.Empty;
        }
    }
}
