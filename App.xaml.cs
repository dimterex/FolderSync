namespace FolderSyns
{
    using FolderSyns.Core;
    using FolderSyns.Interfaces;
    using FolderSyns.Views;

    using Prism.Ioc;
    using Prism.Unity;

    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainUserControlView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IGetFolderManager>(new GetFolderManager());

            containerRegistry.RegisterInstance<IConfigManager>(new ConfigManager());
            var controlSettings = Container.Resolve<IConfigManager>(); 

            containerRegistry.RegisterInstance<ISettingsManager>(new SettingsManager(controlSettings));
            var settingsModel = Container.Resolve<ISettingsManager>();

            containerRegistry.RegisterInstance<ILogManager>(new LogManager(settingsModel));
            var errosSave = Container.Resolve<ILogManager>();

            containerRegistry.RegisterInstance<IHistoryManager>(new HistoryManager(settingsModel, errosSave));
        }
    }
}
