using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1._6_async_voidWpfExample_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /* 窗体加载事件，使用异步加载法
         * 这样就只能写 async void，而不能用 async Task 了
         * 注意：因为 async void 不能传递异常，因此要在异步任务内谨慎处理异常
         */
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //do something
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            /* 
             * 3. 演示死锁，这里 UI 线程阻塞等待子线程返回
             */
            this.label.Content = HeavyJob().Result;
        }

        async Task<int> HeavyJob()
        {
            /*//3. 演示死锁，这里选择回到 UI 线程
            await Task.Delay(2000).ConfigureAwait(true);*/
            //3. 解决死锁，这里选择不回到 UI 线程
            await Task.Delay(2000).ConfigureAwait(false);
            return 10;
        }
    }
}
