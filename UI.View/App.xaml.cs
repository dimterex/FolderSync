using System.Windows;
using DAL;
using DAL.History;
using Core.Dal.Interfaces;
using Core.Manager.Dialog;
using Core.Manager.Event.Interfaces;
using Core.Manager.File;
using Core.Manager.File.Interfaces;
using Core.Manager.History;
using Core.Manager.History.Interfaces;
using Core.Manager.Settings;
using Core.Manager.Settings.Interface;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using UI.View.History;
using UI.View.Main;
using UI.View.Settings;
using UI.ViewModel.Dialog;
using UI.ViewModel.History;
using UI.ViewModel.Locale;
using UI.ViewModel.Locale.Interfaces;
using UI.ViewModel.Main;
using UI.ViewModel.Settings;
using EventManager = Core.Manager.Event.EventManager;

namespace UI.View
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ILocalizationManager>(LocalizationManager.Instance);
            
            containerRegistry.RegisterSingleton<IFolderDialogManager, FolderDialogManager>();

            containerRegistry.RegisterSingleton<ISettingsRepository, SettingsRepository>();
            containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();

            containerRegistry.RegisterSingleton<IEventManager, EventManager>();

            containerRegistry.RegisterSingleton<IHistoryRepository, HistoryRepository>();
            containerRegistry.RegisterSingleton<IHistoryManager, HistoryManager>();

            containerRegistry.RegisterSingleton<IFilesManager, FilesManager>();

            containerRegistry.RegisterSingleton<MainWindowViewModel>();
        }

        protected override void ConfigureViewModelLocator()
        {
            BindViewModelToView<MainView, MainWindowViewModel>();
            BindViewModelToView<HistoryView, HistoryWindowViewModel>();
            BindViewModelToView<SettingsView, SettingsWindowViewModel>();
            base.ConfigureViewModelLocator();
        }

        /// <summary>
        ///     Пробрасывание ViewModel в качестве DataContext View.
        /// </summary>
        private void BindViewModelToView<TView, TViewModel>()
        {
            ViewModelLocationProvider.Register(typeof(TView).ToString(), () => Container.Resolve<TViewModel>());
        }
    }
}