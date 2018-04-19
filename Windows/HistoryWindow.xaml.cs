namespace FolderSyns.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Windows.Data;

    using FolderSyns.Code;

    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow
    {
        public HistoryWindow(List<FileAction> fileActions)
        {
            InitializeComponent();
            HistoryDataGrid.ItemsSource = fileActions;
        }
    }

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

    public class HistoryDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
                return dateTime.ToLocalTime().ToString("f");
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
