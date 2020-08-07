using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using UI.ViewModel.History;

namespace UI.View.Converters
{
    public class HistoryItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is HistoryElementViewModel fileAction))
                return string.Empty;

            var sb = new StringBuilder();
            if (fileAction.IsCopy)
                sb.Append($"{StringResources.WasCopied} {fileAction.NewFolder}");
            if (fileAction.IsCopy && fileAction.IsDelete)
                sb.Append($" {StringResources.And} ");
            if (fileAction.IsDelete)
                sb.Append($"{StringResources.WasRemoved} {fileAction.OldFolder}");
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}