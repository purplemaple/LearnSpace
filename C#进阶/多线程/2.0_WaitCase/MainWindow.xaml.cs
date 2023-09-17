using _2._0_WaitCase.ViewModel;
using System.Windows;

namespace _2._0_WaitCase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new BaseViewModel();
        }
    }
}
