using System.Globalization;
using Prism.Mvvm;
using UI.View;
using UI.ViewModel.Locale.Interfaces;

namespace UI.ViewModel.Locale
{
    public class LocalizationManager : BindableBase, ILocalizationManager
    {
        private StringResources _resources;

        public static LocalizationManager Instance { get; } = new LocalizationManager();

        public StringResources Resources
        {
            get => _resources;
            set => SetProperty(ref _resources, value);
        }

        public LocalizationManager()
        {
            Resources = new StringResources();
        }

        public void SetCulture(CultureInfo cultureInfo)
        {
            StringResources.Culture = cultureInfo;
            RaisePropertyChanged(nameof(Resources));
        }
    }
}