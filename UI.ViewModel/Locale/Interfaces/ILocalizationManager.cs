using System.Globalization;

namespace UI.ViewModel.Locale.Interfaces
{
    public interface ILocalizationManager
    {
        void SetCulture(CultureInfo cultureInfo);
    }
}