namespace FolderSyns.Code
{
    using System;
    using System.IO;

    public static class ErrorSave
    {
        private static string FILE_NAME = "Error.log";

        public static void SaveError(Exception ex)
        {
            //if (!File.Exists(FILE_NAME))
            //{
            //    using (StreamWriter sw = File.CreateText(FILE_NAME))
            //    {
            //        sw.WriteLine($"{ex}");
            //        sw.WriteLine(string.Empty);
            //    }
            //    return;
            //}

            using (StreamWriter sw = File.AppendText(FILE_NAME))
            {
                sw.WriteLine($"{ex}");
                sw.WriteLine(string.Empty);
            }
        }
    }
}
