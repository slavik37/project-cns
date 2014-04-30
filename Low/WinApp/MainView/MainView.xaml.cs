using System.Windows.Controls;

namespace MemOrg.WinApp.MainView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView(MainViewViewModel vm)
        {
            InitializeComponent();
            Loaded += (sender, args) => { DataContext = vm; };
        }
    }
}
