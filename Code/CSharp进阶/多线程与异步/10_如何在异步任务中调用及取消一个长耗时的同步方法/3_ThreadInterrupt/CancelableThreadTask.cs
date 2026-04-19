using System;
using System.Collections.Generic;
using System.Text;

namespace _3_ThreadInterrupt;

/*
 * 仍存在的问题：
 *      `Thread.Interrupt()` 只能在线程进入 Wait / Sleep / Join 等阻塞状态时，触发一个 `ThreadInterruptedException`打断线程，
 *      如果线程生命周期内从未进去这些阻塞状态，那么`Thread.Interrupt()` 将毫无作用
 */

#region CancelableThreadTask 最初的实现
/*
 * 不足之处：
 *    1. 在打断同步任务时可能会有风险，如：同步任务来自类库，这个类库内可能含有一些非托管资源，如果直接打断该同步任务，部分资源可能不会被正确回收
 *    2. `_isRunning`并非线程安全，`RunAsync`仍有被重复调用的风险
 */
//internal class CancelableThreadTask
//{
//    private Thread? _thread;
//    private readonly Action _action;
//    private bool _isRunning = false;

//    //一次性信号量，详见《在异步任务中实现同步机制及使用信号量》
//    //外界 await 等待的就是它，通过 SetResult()、SetCanceled()、SetException() 等设置任务的状态
//    private TaskCompletionSource? _tcs;

//    //传入需要执行的任务，且该任务不能为 null
//    public CancelableThreadTask(Action action)
//    {
//        ArgumentNullException.ThrowIfNull(action);
//        _action = action;

//    }

//    //注意这里不需要标记 async，因为我们不需要在其内使用 await
//    public Task RunAsync(CancellationToken token)
//    {
//        //防止多次调用
//        if (_isRunning)
//            throw new InvalidOperationException("Task is already running!");

//        _isRunning = true;
//        _tcs = new TaskCompletionSource();
//        _thread = new Thread(() =>
//        {
//            try
//            {
//                _action();
//                //在长耗时同步方法完成后设置信号量，外界即可知道任务完成，结束等待
//                _tcs.SetResult();
//            }
//            catch (Exception e)
//            {
//                //捕获到任务被取消
//                if (e is ThreadInterruptedException)
//                    _tcs.TrySetCanceled();  //使用TryTrySetCanceled防止被多次设置
//                                            //并非被取消，而是任务本身抛出异常
//                else
//                    _tcs.SetException(e);
//            }
//            finally
//            {
//                //RunAsync 不管成功与否都可以再次调用
//                _isRunning = false;
//            }
//        });

//        //不需要轮询检查 token，只需要给 token 注册被取消时的回调委托即可
//        //当 token 被取消时打断线程
//        token.Register(() =>
//        {
//            //注：Thread.Interrupt() 并不能确保打断线程
//            //只有线程进入 Wait / Sleep / Join 等阻塞状态时，Interrupt才能触发 `ThreadInterruptedException`异常从而打断线程
//            //对于计算密集型线程，如果其生命周期从未进入这些阻塞状态，那么它不会被打断
//            if (_isRunning)
//            {
//                _thread.Interrupt();
//                //线程被打断后再调用 Join 确保线程结束，避免出现一些资源未被正确回收的风险
//                _thread.Join();
//            }
//        });

//        _thread.Start();
//        return _tcs.Task;
//    }
//}
#endregion

#region 3.1 使用回调委托完善 CancelableThreadTask
//3.1 使用回调委托处理善后工作，如：在被打断时释放资源、在成功完成时执行后续操作等
//internal class CancelableThreadTask
//{
//    private Thread? _thread;
//    private readonly Action _action;
//    //被打断时的回调委托
//    private readonly Action<Exception>? _onError;
//    //成功完成时的回调委托（注：类似于 Task.IsCompleted 与 Task.IsCompletedSuccessfully，完成不等于成功，当 Task 被取消时 IsCompleted 也会为 true）
//    private readonly Action? _onCompletedSuccessfully;
//    private bool _isRunning = false;

//    private TaskCompletionSource? _tcs;

//    //引入回调委托用于处理被打断或成功完成时的善后工作
//    public CancelableThreadTask(Action action, Action<Exception>? onError = null, Action? onCompletedSuccessfully = null)
//    {
//        ArgumentNullException.ThrowIfNull(action);
//        _action = action;
//        _onError = onError;
//        _onCompletedSuccessfully = onCompletedSuccessfully;
//    }

//    public Task RunAsync(CancellationToken token)
//    {
//        if (_isRunning)
//            throw new InvalidOperationException("Task is already running!");

//        _isRunning = true;
//        _tcs = new TaskCompletionSource();
//        _thread = new Thread(() =>
//        {
//            try
//            {
//                _action();
//                _tcs.SetResult();
//                //成功完成时调用委托
//                _onCompletedSuccessfully?.Invoke();
//            }
//            catch (Exception e)
//            {
//                if (e is ThreadInterruptedException)
//                    _tcs.TrySetCanceled();
//                else
//                    _tcs.SetException(e);
//                //处理善后工作
//                _onError?.Invoke(e);
//            }
//            finally
//            {
//                _isRunning = false;
//            }
//        });

//        token.Register(() =>
//        {
//            if (_isRunning)
//            {
//                _thread.Interrupt();
//                //线程被打断后再调用 Join 确保线程结束，避免出现一些资源未被正确回收的风险
//                _thread.Join();
//            }
//        });

//        _thread.Start();
//        return _tcs.Task;
//    }
//}
#endregion

#region 使用原子化操作完善 CancelableThreadTask
//3.2 使用原子化操作`isRunning`确保线程安全
class CancelableThreadTask
{
    private Thread? _thread;
    private readonly Action _action;
    private readonly Action<Exception>? _onError;
    private readonly Action? _onCompletedSuccessfully;

    //private bool _isRunning = false;
    //更改为 int 类型以方便使用 Interlocked 原子化操作
    private int _isRunning = 0;

    private TaskCompletionSource? _tcs;

    public CancelableThreadTask(Action action, Action<Exception>? onError = null, Action? onCompletedSuccessfully = null)
    {
        ArgumentNullException.ThrowIfNull(action);
        _action = action;
        _onError = onError;
        _onCompletedSuccessfully = onCompletedSuccessfully;
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

        //_isRunning = true;
        _tcs = new TaskCompletionSource();
        _thread = new Thread(() =>
        {
            try
            {
                _action();
                _tcs.SetResult();
                _onCompletedSuccessfully?.Invoke();
            }
            catch (Exception e)
            {
                if (e is ThreadInterruptedException)
                    _tcs.TrySetCanceled();
                else
                    _tcs.SetException(e);
                //处理善后工作
                _onError?.Invoke(e);
            }
            finally
            {
                //_isRunning = false;
                //不用比较，直接赋值即可
                Interlocked.Exchange(ref _isRunning, 0);
            }
        });

        token.Register(() =>
        {
            //与开始时相反，这里确保任务正在运行，否则也不需要打断
            if (Interlocked.CompareExchange(ref _isRunning, 0, 1) == 1)
            {
                _thread.Interrupt();
                //线程被打断后再调用 Join 确保线程结束，避免出现一些资源未被正确回收的风险
                _thread.Join();
            }
        });

        _thread.Start();
        return _tcs.Task;
    }
}
#endregion