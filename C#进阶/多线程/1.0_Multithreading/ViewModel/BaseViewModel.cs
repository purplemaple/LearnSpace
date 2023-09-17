using _1._0_Multithreading.Model;
using Prism.Commands;
using Prism;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace _1._0_Multithreading.ViewModel
{
    public class BaseViewModel : BindableBase
    {

        /// <summary>
        /// 同步命令
        /// </summary>
        public DelegateCommand SyncLoadCommand => (new DelegateCommand(() =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResultText = "同步检索开始......\n";
            int idx = 0;

            foreach (Book b in Data.Books)
            {
                AppendLine(++idx + ". " + b.Search());
            }

            sw.Stop();
            AppendLine($"同步检索完成:{Convert.ToSingle(sw.ElapsedMilliseconds) / 1000}秒");
        }));

        



        /// <summary>
        /// 异步命令
        /// </summary>
        public DelegateCommand AsyncLoadCommand => (new DelegateCommand(async () =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResultText = "异步检索开始......\n";
            int idx = 0;

            foreach (Book b in Data.Books)
            {
                /*
                 * 注: 在带有 async 关键字的异步方法中, 遇到 await 时, 主线程会跳出此方法, 继续执行方法外的剩余语句
                 *     当子线程执行完自身 Task 里面的任务后, 并不会立即销毁,  而是会继续执行余下的语句
                 *          此时，如果 Task 设置了 .ConfigureAwait(false) 则余下语句会自动从线程池中找一个线程(一般是当前子线程)继续执行
                 *              如: 
                 *                  下面的两句 AppendLine() 以及 sw.Stop() 都是子线程完成的
                 *                  
                 *               如果 Task 设置了 .ConfigureAwait(true) (默认不设置时也是true) 则余下的语句将会等待主线程来执行(如果主线程又正在等待子线程, 则出现死锁)
                 */

                //string str = await Task.Run(b.Search).ConfigureAwait(true);
                string str = await Task.Run(b.Search);
                AppendLine(++idx + ". " + str);
            }

            sw.Stop();
            AppendLine($"异步检索完成:{Convert.ToSingle(sw.ElapsedMilliseconds) / 1000}秒");
        }));


        /// <summary>
        /// 并发命令
        /// </summary>
        public DelegateCommand ConCurrencyAsyncCommand => (new DelegateCommand(async () =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResultText = "并发检索开始......\n";
            int idx = 0;

            //创立Task集合, 每个Task的返回值都是string
            List<Task<string>> ts = new();
            //遍历Books集合，里面有几本书就开几个线程，并且将所有线程都装入集合中
            foreach (Book book in Data.Books)
            {
                ts.Add(Task.Run(() =>
                {
                    return book.Search();
                }));
            }

            //获取所有线程的返回结果，放入数组中
            string[] strs = await Task.WhenAll(ts);
            foreach (string str in strs)
            {
                AppendLine(++idx + ". " + str);
            }

            sw.Stop();
            AppendLine($"并发检索完成:{Convert.ToSingle(sw.ElapsedMilliseconds) / 1000}秒");
        }));

        /// <summary>
        /// 异步回调命令
        /// </summary>
        public DelegateCommand AsyncCallbackCommand => (new DelegateCommand(async () =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResultText = "异步回调检索开始......\n";
            int idx = 0;

            List<Task> ts = new();

            foreach (Book book in Data.Books)
            {
                ts.Add(Task.Run(book.Search).ContinueWith(t =>
                {
                    /*
                     * System.Windows.Application.Current.Dispatcher
                     * 获取当前应用程序的主窗口的调度器对象, 然后使用Invoke()来执行UI相关的操作
                     */
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        AppendLine(++idx + ". " + t.Result);
                    });
                }));
            }

            await Task.WhenAll(ts);
            sw.Stop();
            AppendLine($"异步回调检索完成:{Convert.ToSingle(sw.ElapsedMilliseconds) / 1000}秒");
        }));

        /*
         * (wpf基本不写事件，作废)
        /// <summary>
        /// 事件命令 
        /// </summary>
        public DelegateCommand EventCommand => (new DelegateCommand(async() =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResultText = "同步检索开始......\n";
            int idx = 0;

            //创立Task集合, 每个Task的返回值都是string
            List<Task> ts = new();
            //遍历Books集合，里面有几本书就开几个线程，并且将所有线程都装入集合中
            foreach (Book book in Data.Books)
            {
                //ts.Add(Task.Run(book.SearchEvent));
            }

            //获取所有线程的返回结果，放入数组中
            await Task.WhenAll(ts);

            sw.Stop();
            AppendLine($"同步检索完成:{Convert.ToSingle(sw.ElapsedMilliseconds) / 1000}秒");
        }));
        */

        private void AppendLine(string text)
        {
            ResultText += text;
            ResultText += Environment.NewLine;
        }

        private string _resultText;
        public string ResultText
        {
            get => _resultText;
            set
            {
                if (_resultText != value)
                {
                    SetProperty(ref _resultText, value);
                }
            }
        }

    }
}
