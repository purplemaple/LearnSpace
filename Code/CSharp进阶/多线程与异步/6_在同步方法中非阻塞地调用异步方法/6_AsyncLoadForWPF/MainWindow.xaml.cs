using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/*
 * 6. 在 WPF 中绑定异步任务
 * 如：打开窗口后异步加载数据（不卡UI），加载完成后展示
 */

namespace _6_AsyncLoadForWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModel();
            this.DataContext = _viewModel;
        }

        //1. 在 Loaded 事件中调用 ViewModel 的异步加载数据方法 (需要在 xaml 中注明 Loaded="Window_Loaded" 事件)
        //private async Task Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    await _viewModel.LoadDataAsync();
        //}
    }
}