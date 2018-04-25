namespace FolderSyns.Windows
{
    using System.Diagnostics;

    using FolderSyns.MVVM.MainUserControl;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainUserControlViewModel = MainUserControlViewModel.Instance;
            MainUserControlView.DataContext = mainUserControlViewModel;
            Closed += (sender, args) => { mainUserControlViewModel.CloseCommand.Execute();} ;
        }
    }
}
