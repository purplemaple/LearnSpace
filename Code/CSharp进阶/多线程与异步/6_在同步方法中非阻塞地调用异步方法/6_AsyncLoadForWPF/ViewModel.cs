using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace _6_AsyncLoadForWPF;

internal partial class ViewModel : ObservableObject
{

    //2. 使用 EventToCommand 绑定 Loaded 事件到 Command (需要在 xaml 中使用 EventTrigger、InvokeCommandAction)

    //2.1 手搓 IAsyncRelayCommand
    //public IAsyncRelayCommand LoadDataCommand { get; }

    public ViewModel()
    {
        //2.1 手动声明 AsyncRelayCommand
        //LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
    }

    //2.2 使用 CommunityToolkit 自带的 RelayCommand
    //对于 Prism, 原生的 DelegateCommand 更偏向同步命令，如果项目里大量存在异步命令，可以自己封装一个 AsyncDelegateCommand

    [RelayCommand]  //同步异步皆可用，异步方法时，框架会自动生成 IAsyncRelayCommand 类型的 Command
    public async Task LoadDataAsync()   //注：RelayCommand生成的命令名会自动去除末尾的 Async，因此 xaml 中应该绑定 LoadDataCommand
    {
        await Task.Delay(1500);
    }

    /*
     * 3. 扩展：CommunityToolkit 提供的 TaskNotifier，本质是 Task 的包装器，可以用来监视异步操作的状态（如是否正在运行、是否完成、是否有异常等）
     * 并且可以绑定到 UI 上，以便根据异步操作的状态来更新界面。
     * 如：前台某些控件的显示状态与 Task.IsRunning 进行绑定（AsyncRelayCommand 本身也有 IsRunning 属性可供绑定）
     * 
     * 关键：
     *      1. 当 Task 完成时，它会再次触发 PropertyChanged
     *      2. 需要配合 SetPropertyAndNotifyOnCompletion 使用
     * 
     * 解决的问题：
     *      普通的 Task 完成时不会触发 PropertyChanged，UI无法自动刷新，而 TaskNotifier 能自动监听异步任务完成并触发 PropertyChanged。
     */
    TaskNotifier? taskNotifier;
}
