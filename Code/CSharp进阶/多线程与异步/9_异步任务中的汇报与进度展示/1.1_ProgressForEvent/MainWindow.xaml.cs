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

namespace _1._1_ProgressForEvent
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                button.IsEnabled = false;
                //2.0 创建 Progress 类对象并传入回调委托
                //Progress<double> progress = new(value => progressBar.Value = value);

                //2.1 创建自定义 Progress 类对象并传入进度条完成时的回调
                MyProgress<double> myProgress = new(
                    _ => progressBar.Value += 1, 
                    () => progressBar.Visibility = Visibility.Hidden, 
                    100);
                //await DoJobAsync(myProgress);

                //3.1 在传统多线程中使用
                //await Task.Run(() => Parallel.For(1, 100, value =>
                //{
                //    //注：Random.Shared 从 .Net 6.0 引进，是一个线程安全的 Random 实例
                //    Thread.Sleep(Random.Shared.Next(500));
                //    ((IProgress<double>)myProgress).Report(value);
                //}));

                //3.2 使用.Net 8.0 新增的 ForAsync
                await Parallel.ForAsync(1, 100, async (value, _) =>
                {
                    //注：Random.Shared 从 .Net 6.0 引进，是一个线程安全的 Random 实例
                    Thread.Sleep(Random.Shared.Next(500));
                    ((IProgress<double>)myProgress).Report(value);
                });

                button.IsEnabled = true;
            }
            catch (Exception)
            {

            }
        }

        async Task DoJobAsync(IProgress<double> myProgress)
        {
            for (int i = 1; i <= 100; i++)
            {
                //1.0 模拟非UI线程操作UI资源的情况
                await Task.Delay(50).ConfigureAwait(false);
                //1.1 非UI线程直接使用UI资源时会出异常
                //progressBar.Value = i;
                //1.2 使用Dispatcher在UI线程上操作UI资源
                //Dispatcher.Invoke(() => progressBar.Value = i);

                //2.0 汇报（即执行传入的回调）
                myProgress.Report(i);
            }
        }
    }

    //2.1 自定义 Progress
    class MyProgress<T> : Progress<T> where T : notnull
    {
        private readonly Action? _complete;
        private readonly T _maximum;
        private bool _isCompleted;
        private readonly SynchronizationContext _synchronizationContext;

        /// <summary>
        /// 自定义 Progress 的构造
        /// </summary>
        /// <param name="handler">每次 Report 时的回调</param>
        /// <param name="complete">这里指进度条走完后执行的回调</param>
        /// <param name="maximum">进度条最大值</param>
        public MyProgress(Action<T> handler, Action? complete, T maximum): base(handler)
        {
            _complete = complete;
            _maximum = maximum;
            //注册委托
            ProgressChanged += CheckCompletion;

            _synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
        }

        protected override void OnReport(T value)
        {
            //如果进度条已走完则跳过汇报
            if (_isCompleted)
                return;
            base.OnReport(value);
        }

        //用于检测当前进度条是否已经走完
        private void CheckCompletion(object? sender, T e)
        {
            if (e.Equals(_maximum) && !_isCompleted)
            {
                _isCompleted = true;
                //执行完成回调
                _complete?.Invoke();
            }
        }
    }
}