using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace _1_AsyncBarrier;

public partial class MainViewModel : ObservableObject
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

    /*
     * 补充点二：RelayCommand 有不少重载
     *      其中 IncludeCancelCommand 参数设为 true 会自动生成一个对应的 MethodNameCancelCommand，使用这个 CancelComman.Execute 即可取消原方法(需要原方法携带 CancellationToken 参数)
     */
    [RelayCommand(IncludeCancelCommand = true)]
    async Task FirstJobAsync(CancellationToken token)
    {
        InitJobs();
        /*
         * 补充点三：选中代码块按 Ctrl + k + s 可以快速生成代码片段
         */
        try
        {
            await Task.Delay(1500, token);
            Results.Add("First job is almost completed. Waiting for async barrier...");
            //await _asyncBarrier.SignalAndWait(token);
            await AsyncBarrier.SignalAndWait(token);
            Results.Add("First job is completed.");
            FinishJobs(true);
        }
        catch (TaskCanceledException)
        {
            Results.Add("First job was cancelled.");
            FinishJobs(false);
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    async Task SecondJobAsync(CancellationToken token)
    {
        InitJobs();
        try
        {
            await Task.Delay(1500, token);
            Results.Add("Second job is almost completed. Waiting for async barrier....");
            //await _asyncBarrier.SignalAndWait(token);
            await AsyncBarrier.SignalAndWait(token);
            Results.Add("Second job is completed.");
            FinishJobs(true);
        }
        catch (TaskCanceledException)
        {
            Results.Add("Second job was cancelled.");
            FinishJobs(false);
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    async Task ThirdJobAsync(CancellationToken token)
    {
        InitJobs();
        try
        {
            await Task.Delay(1500, token);
            Results.Add("Third job is almost completed. Waiting for async barrier....");
            //await _asyncBarrier.SignalAndWait(token);
            await AsyncBarrier.SignalAndWait(token);
            Results.Add("Third job is completed.");
            FinishJobs(true);
        }
        catch (TaskCanceledException)
        {
            Results.Add("Third job was cancelled.");
            FinishJobs(false);
        }
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
        if (FirstJobCommand.IsRunning) FirstJobCommand.Cancel();
        if (SecondJobCommand.IsRunning) SecondJobCommand.Cancel();
        if (ThirdJobCommand.IsRunning) ThirdJobCommand.Cancel();
    }

    /*
     * 补充点一: MemberNotNull 这个特性出自 System.Diagnostics.CodeAnalysis 命名空间，表示在调用该方法后，指定的成员将不为 null，比直接用 ! 更加安全和明确。
     *      其中 System.Diagnostics.CodeAnalysis 常用于代码分析和静态检查。
     */
    [MemberNotNull(nameof(AsyncBarrier))]
    void InitJobs()
    {
        if (AsyncBarrier is null)
        {
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
            AsyncBarrier = null;
            if (success) 
                Results.Add("All jobs are completed.");
            else
                Results.Add("All jobs are cancelled.");
        }

        //手动触发通知
        //CancelAllJobsCommand.NotifyCanExecuteChanged();
    }
}
