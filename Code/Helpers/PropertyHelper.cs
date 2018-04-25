namespace FolderSyns.Code.Helpers
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public static class PropertyHelper
    {
        private static event PropertyChangedEventHandler PropertyChanged;

        public static void SetProperty<T>(ref T oldValue, T newValue, object sender, string propName)
        {
            oldValue = newValue;
            OnPropertyChanged(sender, propName);
        }

        private static void OnPropertyChanged(object sender, [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
