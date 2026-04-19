using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace _2_AsyncBarrierUpgrade;

internal partial class MainViewModel : ObservableObject
{
    public ObservableCollection<string> Results { get; } = [];

    /*
     * 补充五：使用 [NotifyCanExecuteChangedFor(nameof())]，在属性变更时自动通知对应的 Command
     *      这个特性需要标注在属性上, 因此这里同时使用 [ObservableProperty] 生成对应的属性
     * 
     *      逻辑: 当 AsyncBarrier 属性被设置 -> CancelAllJobsCommand.NotifyCanExecuteChanged() -> 触发 CanExecuteChanged 事件 -> 重新调用 CanExecute() -> 得到最新状态 -> 更新 UI
     */
    [NotifyCanExecuteChangedFor(nameof(CancelAllJobsCommand))]
    [ObservableProperty]
    private AsyncBarrier? _asyncBarrier;

    private CancellationTokenSource? _cts;

    /*
     * 补充点二：RelayCommand 有不少重载
     *      其中 IncludeCancelCommand 参数设为 true 会自动生成一个对应的 MethodNameCancelCommand，使用这个 CancelComman.Execute 即可取消原方法(需要原方法携带 CancellationToken 参数)
     */
    [RelayCommand]
    async Task FirstJobAsync()
    {
        InitJobs();
        /*
         * 补充点三：选中代码块按 Ctrl + k + s 可以快速生成代码片段
         */
        try
        {
            await Task.Delay(1500, _cts.Token);
            //throw new Exception("First job encountered an exception.");
            
            Results.Add("First job is almost completed. Waiting for async barrier...");
            //await _asyncBarrier.SignalAndWait(token);
            await AsyncBarrier.SignalAndWait(_cts.Token);
            Results.Add("First job is completed.");
        }
        catch (AsyncBarrierBrokenException ex)
        {
            Results.Add($"An exception happened in First job due to barrier broken. reason: {ex.InnerException?.Message}");
        }
        catch (TaskCanceledException)
        {
            /* 
             * 当 AsyncBarrier 已经被 Break 时，如果正好有任务被取消，这里区分语义。
             * 根据 chatGPT 的回答，即使 IsBroken 使用了 lock 语句，但仍然不能保证时序一致性，仍有可能某个任务先于 break 获取 IsBroken 状态，走了普通的 Cancel，因此只能减少而不能避免竞态的发生
             */
            if (AsyncBarrier.IsBroken)
            {
                Results.Add("First job was cancelled due to barrier broken.");
            }
            else
            {
                Results.Add("First job was cancelled.");
            }
            FinishJobs(false);
        }
        catch (Exception ex)
        {
            //在新的 AsyncBarrier 中，Break 方法可以接受一个异常参数，这个异常会被传递给所有等待的任务，使它们能够了解取消的原因
            //AsyncBarrier.Break(new AsyncBarrierBrokenException("First job encountered an exception.", ex));
            AsyncBarrier.Break(ex);
            Results.Add("some exception happened in First job, breaking the barrier. ");
            _cts.Cancel(); //这里可以选择直接取消所有任务，或者让其他任务通过 AsyncBarrierBrokenException 来感知异常并自行决定是否取消
        }

        //FinishJobs(true);
    }

    [RelayCommand]
    async Task SecondJobAsync()
    {
        InitJobs();
        try
        {
            await Task.Delay(1500, _cts.Token);
            throw new Exception("Second job encountered an exception.");

            Results.Add("Second job is almost completed. Waiting for async barrier....");
            //await _asyncBarrier.SignalAndWait(token);
            await AsyncBarrier.SignalAndWait(_cts.Token);
            Results.Add("Second job is completed.");
        }
        catch (AsyncBarrierBrokenException ex)
        {
            Results.Add($"An exception happened in Second job due to barrier broken. reason: {ex.InnerException?.Message}");
        }
        catch (TaskCanceledException)
        {
            /* 
             * 当 AsyncBarrier 已经被 Break 时，如果正好有任务被取消，这里区分语义。
             * 根据 chatGPT 的回答，即使 IsBroken 使用了 lock 语句，但仍然不能保证时序一致性，仍有可能某个任务先于 break 获取 IsBroken 状态，走了普通的 Cancel，因此只能减少而不能避免竞态的发生
             */
            if (AsyncBarrier.IsBroken)
            {
                Results.Add("Second job was cancelled due to barrier broken.");
            }
            else
            {
                Results.Add("Second job was cancelled.");
            }
            FinishJobs(false);
        }
        catch (Exception ex)
        {
            //在新的 AsyncBarrier 中，Break 方法可以接受一个异常参数，这个异常会被传递给所有等待的任务，使它们能够了解取消的原因
            //AsyncBarrier.Break(new AsyncBarrierBrokenException("Second job encountered an exception.", ex));

            AsyncBarrier.Break(ex);
            Results.Add($"some exception happened in Second job, breaking the barrier. reason: {ex.Message}");

            await Task.Yield(); //确保在 Break 之后再触发 Cancel，这样等待的任务能先感知到 Break 的异常，再感知到 Cancel 的取消，减少竞态的发生
            _cts.Cancel(); //这里可以选择直接取消所有任务，或者让其他任务通过 AsyncBarrierBrokenException 来感知异常并自行决定是否取消
        }

        //FinishJobs(true);
    }

    [RelayCommand]
    async Task ThirdJobAsync()
    {
        InitJobs();
        try
        {
            await Task.Delay(3500, _cts.Token);
            Results.Add("Third job is almost completed. Waiting for async barrier....");
            //await _asyncBarrier.SignalAndWait(token);
            await AsyncBarrier.SignalAndWait(_cts.Token);
            Results.Add("Third job is completed.");
        }
        catch (AsyncBarrierBrokenException ex)
        {
            Results.Add($"An exception happened in Third job due to barrier broken. reason: {ex.InnerException?.Message}");
        }
        catch (TaskCanceledException)
        {
            /* 
             * 当 AsyncBarrier 已经被 Break 时，如果正好有任务被取消，这里区分语义。
             * 根据 chatGPT 的回答，即使 IsBroken 使用了 lock 语句，但仍然不能保证时序一致性，仍有可能某个任务先于 break 获取 IsBroken 状态，走了普通的 Cancel，因此只能减少而不能避免竞态的发生
             */
            if (AsyncBarrier.IsBroken)
            {
                Results.Add("Third job was cancelled due to barrier broken.");
            }
            else
            {
                Results.Add("Third job was cancelled.");
            }
            FinishJobs(false);
        }
        catch (Exception ex)
        {
            //在新的 AsyncBarrier 中，Break 方法可以接受一个异常参数，这个异常会被传递给所有等待的任务，使它们能够了解取消的原因
            AsyncBarrier.Break((ex));
            Results.Add("some exception happened in Third job, breaking the barrier. ");
            _cts.Cancel(); //这里可以选择直接取消所有任务，或者让其他任务通过 AsyncBarrierBrokenException 来感知异常并自行决定是否取消
        }

        //FinishJobs(true);
    }

    /*
     * 补充四：RelayCommand 的 CanExecute 可以直接绑定一个 bool 方法/属性，这样当 Command 的 CanExecute 状态改变时 UI 会自动更新对应的按钮状态
     *      注意：需要调用 CanCancel.NotifyCanExecuteChanged() 来触发通知，想要不手动通知，可参考补充五
     */
    bool CanCancel => AsyncBarrier is not null;

    [RelayCommand(CanExecute = nameof(CanCancel))]
    void CancelAllJobs()
    {
        //使用 [RelayCommand(IncludeCancelCommand = true)] 即可生成此方法用于取消任务
        //if (FirstJobCommand.IsRunning) FirstJobCancelCommand.Execute(null);
        //if (SecondJobCommand.IsRunning) SecondJobCancelCommand.Execute(null);
        //if (ThirdJobCommand.IsRunning) ThirdJobCancelCommand.Execute(null);

        //这里也可以直接调用Command的 Cancel() 来取消任务，效果是一样的
        //if (FirstJobCommand.IsRunning) FirstJobCommand.Cancel();
        //if (SecondJobCommand.IsRunning) SecondJobCommand.Cancel();
        //if (ThirdJobCommand.IsRunning) ThirdJobCommand.Cancel();

        /*
         * 注：以上都是 UI Command 层取消，如果任务还没进入 await （比如 CPU 密集型操作），Cancel 不一定立即生效。
         * 因此引入一个共享的 CancellationTokenSource，在 InitJobs 时创建，在 FinishJobs 时取消，这样所有任务都共享同一个 CancellationToken，可以更及时地响应取消请求。
         */
        _cts?.Cancel();
    }

    /*
     * 补充点一: MemberNotNull 这个特性出自 System.Diagnostics.CodeAnalysis 命名空间，表示在调用该方法后，指定的成员将不为 null，比直接用 ! 更加安全和明确。
     *      其中 System.Diagnostics.CodeAnalysis 常用于代码分析和静态检查。
     */
    [MemberNotNull(nameof(_cts))]
    [MemberNotNull(nameof(AsyncBarrier))]
    void InitJobs()
    {
        if (_cts == null)
            _cts = new CancellationTokenSource();
        if (AsyncBarrier is null)
        {
            //初始化共享 CancellationTokenSource ，所有传入 AsyncBarrier 的 任务都使用这个源，这样在 CancelAllJobs 时可以更及时地取消所有任务
            AsyncBarrier = new AsyncBarrier(3);
            Results.Clear();
        }

        //手动触发通知
        //CancelAllJobsCommand.NotifyCanExecuteChanged();
    }

    void FinishJobs(bool success = true)
    {
        if (AsyncBarrier is not null)
        {    
            if (success)
            {
                Results.Add("All jobs are completed.");
                AsyncBarrier = null;
            }
            else
            {
                Results.Add("All jobs are cancelled.");
            }
        }

        _cts?.Dispose();
        _cts = null;

        //手动触发通知
        //CancelAllJobsCommand.NotifyCanExecuteChanged();
    }
}
