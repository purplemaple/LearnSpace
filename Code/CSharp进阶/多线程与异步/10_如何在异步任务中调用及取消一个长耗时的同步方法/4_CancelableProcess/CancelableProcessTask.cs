using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace _4_CancelableProcess;

/* 
 * 4.3 主程序的异步任务类，封装了一个 Process，能够在取消时正确地杀死进程，并且能够正确地处理进程的退出状态
 * Process 具体执行第三方类库的哪个方法由构造中的 arguments 决定，如 "cancelable" 执行 ThirdPartyUtils.CancelableSyncMethod()...
 * 
 */
internal class CancelableProcessTask
{
    private Process _process;

    //一次性信号量，详见《在异步任务中实现同步机制及使用信号量》
    //外界 await 等待的就是它，通过 SetResult()、SetCanceled()、SetException() 等设置任务的状态
    private TaskCompletionSource? _tcs;

    //int 类型以方便使用 Interlocked 原子化操作
    private int _isRunning = 0;

    public CancelableProcessTask(string filename, string arguments)
    {
        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = filename,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                //不会弹出控制台窗口
                CreateNoWindow = true
            }
        };
    }

    public Task RunAsync(CancellationToken token)
    {
        /*
		 * CompareExchange 的参数：location1(初始值)、value(准备赋予的值)、comparand(被比较值)，返回值：未经修改的初始值
		 * location1 将与 comparand 进行比较，如果不同，则将 value 的值赋予 location1
		 * 无论是否赋值，都返回未经修改的初始值
		 */
        if (Interlocked.CompareExchange(ref _isRunning, 1, 0) == 1)
            throw new InvalidOperationException("Task is already running!");

        //一次性信号量，外界 await 等待的就是它
        _tcs = new TaskCompletionSource();

        //注册任务取消时的回调，确保在取消时能够正确地杀死进程
        token.Register(() =>
        {
            if (Interlocked.CompareExchange(ref _isRunning, 0, 1) == 1)
            {
                try
                {
                    if (!_process.HasExited)
                    {
                        _process.Kill();
                        //我们会在进程退出时的 Exited 事件中设置任务为已取消状态，因此这里不需要 TrySetCanceled
                        //_tcs.TrySetCanceled(token);
                    }
                }
                catch (Exception e)
                {
                    //杀进程时出现异常，可能没有权限杀死进程等
                    _tcs.TrySetException(e);
                }
            }
        });

        //开启后续事件通知，当进程退出时会触发 Exited 事件
        _process.EnableRaisingEvents = true;
        //订阅 Process 的 Exited 事件，以便在进程结束时执行一些 任务
        _process.Exited += (sender, args) =>
        {
            if (_process.ExitCode == 0) //进程正常退出(一般程序退出码为 0 表示正常退出)
                _tcs.SetResult();
            else                        //进程退出失败有两种情况：1. 被取消；2. 进程本身抛异常
            {
                if (token.IsCancellationRequested)
                    _tcs.SetCanceled(); //如果是因为取消而退出，设置任务为已取消状态
                else
                    _tcs.SetException(new Exception($"Process exited with code {_process.ExitCode}"));
            }
            Interlocked.Exchange(ref _isRunning, 0);
        };

        try
        {
            _process.Start();
        }
        catch (Exception e)
        {
            Interlocked.Exchange(ref _isRunning, 0);
            _tcs.SetException(e);   //进程启动失败，设置任务为异常状态
        }
        return _tcs.Task;
    }
}
