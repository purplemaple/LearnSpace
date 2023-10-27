using _2._0_WaitCase.Model;
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

namespace _2._0_WaitCase.ViewModel
{
    public class BaseViewModel : BindableBase
    {

        /// <summary>
        /// Wait命令
        /// </summary>
        public DelegateCommand WaitCommand => (new DelegateCommand(() =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResultText = "Wait方法开始......\n";
            int idx = 0;

            foreach (Book b in Data.Books)
            {
                Task<string> task = Task.Run(() =>
                {
                    return b.Search();
                });
                idx++;
                /*
                 * Task.Wait()方法, 调用线程阻塞在Wait处, 出现两种情况结束等待
                 * 1. 线程执行完毕
                 * 2. 任务本身已取消或引发异常
                 */
                task.Wait();        //注：主线程会阻塞在这里，因此和同步法没什么区别
                AppendLine(idx + ". " + task.Result);
            }

            sw.Stop();
            AppendLine($"Wait方法完成:{Convert.ToSingle(sw.ElapsedMilliseconds) / 1000}秒");
        }));





        /// <summary>
        /// WaitAll命令
        /// </summary>
        public DelegateCommand WaitAllCommand => (new DelegateCommand(async () =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResultText = "WaitAll开始......\n";
            int idx = 0;

            List<Task<string>> ts = new();
            foreach (Book b in Data.Books)
            {
                ts.Add(Task.Run(() =>
                {
                    return b.Search();
                }));
            }
            /*
             * Task.WaitAll()方法, ts 线程集合中全部线程结束后, 主线程才向后执行
             * 与 Wait() 相同, 只不过这里参数是 Task 数组, 用了并发方法，所以执行比同步法快
             */
            Task.WaitAll(ts.ToArray());

            foreach (Task<string> task in ts)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AppendLine(task.Result + " " + task.Id + ". " + task.Status);
                });
            }

            sw.Stop();
            AppendLine($"WaitAll完成:{Convert.ToSingle(sw.ElapsedMilliseconds) / 1000}秒");
        }));


        /// <summary>
        /// WaitAny命令
        /// </summary>
        public DelegateCommand WaitAnyCommand => (new DelegateCommand(async () =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResultText = "WaitAny开始......\n";
            int idx = 0;

            List<Task<string>> ts = new();
            foreach (Book b in Data.Books)
            {
                ts.Add(Task.Run(() =>
                {
                    return b.Search();
                }));
            }
            /*
             * Task.WaitAny()方法, ts 线程集合中任一线程结束后, 主线程就继续向后执行
             * 
             */
            Task.WaitAny(ts.ToArray());

            foreach (Task<string> task in ts)
            {
                //AppendLine(task.Id + ". " + task.Status);
                /*
                 * 注: 使用 WaitAny() 方法时, 如果主线程遇到 task.Result 这样需要线程返回结果的情况时
                 * 主线程就会等待这个线程结束，拿到结果后再运行
                 * 
                 * 例: 
                 *      封神演义----用时: 1秒
                 *      三国演义----用时: 2秒
                 *      水浒传  ----用时: 3秒
                 *      西游记  ----用时: 4秒     所有线程并发执行, 而这个线程执行时间最久, 主线程卡到他结束拿到结果后, 其他线程已全部结束, 因此主线程可以直接拿到全部结果
                 *      红楼梦  ----用时: 2秒
                 *      聊斋志异----用时: 1秒
                 *      儒林外史----用时: 2秒
                 *      隋唐演义----用时: 1秒
                 * 
                 */
                AppendLine(task.Result + " " + task.Id + ". " + task.Status);
            }

            sw.Stop();
            AppendLine($"WaitAny完成:{Convert.ToSingle(sw.ElapsedMilliseconds) / 1000}秒");
        }));


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
