namespace FolderSyns.MVVM.MainUserControl
{

    /// <summary>
    /// Логика взаимодействия для MainUserControl.xaml
    /// </summary>
    public partial class MainUserControlView
    {
        public MainUserControlView()
        {
            var mainUserControlViewModel = MainUserControlViewModel.Instance;
            DataContext = mainUserControlViewModel;
            InitializeComponent();
            Closed += (sender, args) => { mainUserControlViewModel.CloseCommand.Execute(); };
        }
    }
}
