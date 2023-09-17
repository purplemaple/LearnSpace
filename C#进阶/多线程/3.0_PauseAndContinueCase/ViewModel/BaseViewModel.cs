using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _3._0_PauseAndContinueCase.ViewModel
{
    public class BaseViewModel : BindableBase
    {
        private CancellationTokenSource? TokenSource;
        private ManualResetEvent? ManualReset;

        private void Print(string text)
        {
            
        }

        public DelegateCommand StartCommand => (new DelegateCommand(() =>
        {
            TokenSource = new();
            ManualReset = new(true);
            int i = 0;
            Task.Run(() =>
            {
                while (!TokenSource.Token.IsCancellationRequested)
                {
                    ManualReset.WaitOne();      //当 ManualReset 为 false 时, 阻塞当前线程
                    Thread.Sleep(200);

                }
            }, TokenSource.Token);
        }));

        /// <summary>
        /// 暂停命令
        /// </summary>
        public DelegateCommand PauseCommand => (new DelegateCommand(() =>
        {
            ManualReset?.Reset();               //
        }));

        /// <summary>
        /// 继续命令
        /// </summary>
        public DelegateCommand ContinueCommand => (new DelegateCommand(() =>
        {
            ManualReset?.Set();
        }));

        /// <summary>
        /// 停止命令
        /// </summary>
        public DelegateCommand StopCommand => (new DelegateCommand(() =>
        {
            TokenSource?.Cancel();
        }));

        private void AppendLinne(string text)
        {

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
