namespace FolderSyns.Code.Helpers
{
    using System.Windows.Forms;

    public enum FolderType
    {
        SourceFolder,
        TargetFolder,
        FolderForHistory,

    }

    public static class OpenFolderDialog
    {
        /// <summary>
        /// Указать папку.
        /// </summary>
        /// <folderPath name="folderPath">Указывается первая папка.</folderPath>
        public static string OpenFolderPath(string folderPath)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(folderPath))
                folderBrowserDialog.SelectedPath = folderPath;

            // ??? folderBrowserDialog.Description = "Укажите папку";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }

            return string.Empty;

        }





    }
}
