using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace _2._1_IAsyncEnumberableForMVVM.ViewModel;

internal partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    int progressValue = 0;

    [ObservableProperty]
    int progressMaximum = 100;

    [ObservableProperty]
    string? progressText;

    public MainViewModel() { }

    [RelayCommand]
    public async Task StartWork()
    {
        //await foreach (var progress in DoJobAsync(ProgressMaximum))
        //{
        //    ProgressValue = progress;
        //}

        //异步枚举语法，会在每次 yield return 时收到一个进度
        await foreach (ProgressInfo info in DoJobWithInfoAsync())
        {
            ProgressValue = info.Step;
            ProgressText = info.Message;
        }
    }

    async IAsyncEnumerable<int> DoJobAsync()
    {
        for (int i = 1; i <= ProgressMaximum; i++)
        {
            await Task.Delay(50);
            //每次遇到 yield return 时，都会将当前的值返回给调用方（上层消费者），并暂停方法的执行状态，直到下一次请求下一个值时才继续执行
            yield return i;
        }
    }
    
    async IAsyncEnumerable<ProgressInfo> DoJobWithInfoAsync()
    {
        for (int i = 1; i <= ProgressMaximum; i++)
        {
            await Task.Delay(50);
            //每次遇到 yield return 时，都会将当前的值返回给调用方（上层消费者），并暂停方法的执行状态，直到下一次请求下一个值时才继续执行
            yield return new ProgressInfo(i, $"当前进度：{i}/{ProgressMaximum}");
        }
    }
    
    record ProgressInfo(int Step, string Message);
}
