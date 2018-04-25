namespace FolderSyns.Conderters
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows.Data;

    using FolderSyns.Code;

    public class HistoryItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is FileAction fileAction))
                return string.Empty;

            var sb = new StringBuilder();
            if (fileAction.IsCopy)
                sb.Append($"был скопирован в {fileAction.NewFolder}");
            if (fileAction.IsCopy && fileAction.IsDelete)
                sb.Append(" и ");
            if (fileAction.IsDelete)
                sb.Append($"был удален с {fileAction.OldFolder}");
            return sb.ToString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
