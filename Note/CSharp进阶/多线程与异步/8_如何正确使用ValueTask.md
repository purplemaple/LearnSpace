
#### 解决的问题：异步任务提前结束（没有进行到`await`那句），看似异步实则同步时，`Task`造成浪费内存的问题


## 1. 传统 `Task` 类型的问题

示例：

```csharp
class MyService
{
	//模拟缓存的
    private readonly Dictionary<int, string> _cache;

    public MyService()
    {
        _cache = new()
        {
            [42] = "hello",
        };
    }

    public async Task<string> GetResultAsync(int id)
    {
        /* 
         * 如果实际业务中缓存命中情况较多时，会提前放回，而不会进行下面的 await 异步语句
         * 但是外部 await 这个方法时，编译器仍会将其转换成一个Task状态机，导致即使缓存命中，也会有额外的性能开销（约136B以上）
         * 
         * 这种在异步任务中直接返回一个值的情况，我们称之为“同步完成”，或者“返回同步结果”。线程进入这个异步任务后，并没有碰到 await 关键字，而是直接返回。也就是说，这个异步任务自始至终都是在同一个线程上执行的。
         */
        if (_cache.TryGetValue(id, out var result))
        {
            return result;
        }

        await Task.Delay(1000);
        return id.ToString();
    }
}
```

在上述的`GetResultAsync`中，首先考虑从缓存中获取消息，其次再考虑从数据库中获取，此时引发一个问题：如果缓存命中直接返回，但我们仍会返回一个`Task<string>` 类型的引用对象，导致内存的无端消耗

> info：
> 这种在异步任务中直接返回一个值的情况，我们称之为“同步完成”，或者“返回同步结果”。线程进入这个异步任务后，并没有碰到 await 关键字，而是直接返回。也就是说，这个异步任务自始至终都是在同一个线程上执行的。

---

## 2. `ValueTask`

代码几乎不需修改：

```csharp
class MyService
{
    //模拟缓存的一些数据
    private readonly Dictionary<int, string> _cache;

    public MyService()
    {
        _cache = new()
        {
            [42] = "hello",
        };
    }

    /*
     * ValueTask 顾名思义是一个值类型，被分配在栈上，避免了 Task 对象的堆分配开销。
     * 当使用 ValueTask 时，如果方法能够同步完成（例如缓存命中），它会直接返回一个结果（0B），而不需要创建一个 Task 对象。这就大大减少了内存分配和垃圾回收的压力。（若仍需异步完成，则会自动创建 Task 对象返回（144B以上））
     * 
     * 注：Task 与 ValueTask 的关系不同于 Tuple 与 ValueTuple 的关系，ValueTask 并非 Task 的值类型版本。最佳理解应是：Value and Task -> 有时会返回 value 有时会返回 Task
     */
    public async ValueTask<string> GetResultWithValueTaskAsync(int id)
    {
        //缓存命中，直接返回 ValueTask<T> 对象，而不需要再创建一个 Task<T> 对象，避免内存无端消耗
        if (_cache.TryGetValue(id, out var result))
        {
            return result;
        }

        //没有命中缓存，创建 Task<T> 对象并返回（这种情况比直接使用 Task 时稍微多了一点开销）
        await Task.Delay(1000);
        return id.ToString();
    }
}
```

此时，如果缓存中有数据，那么我们可以直接返回一个 `ValueTask<T>` 类型的值对象，而不需要再创建一个 `Task<T>` 对象。这样就避免了无端的堆内存开销；
否则，我们才会创建 `Task<T>` 对象。或者说，在这种情况下，`ValueTask` 的性能会退化为 `Task`（甚至可能还稍微低一丁点，因为涉及到更多的字段，以及值拷贝等）。

>info
>`ValueTask`最初只有泛型版本，后来在.NET 7中引入了非泛型版本。但非泛型版本的 `ValueTask`，的使用情形更少。它只有在即使异步完成也可以无需分配内存的情况下才会派上用场。除非你借助 profiling 工具确认 `Task` 的这一丁点开销会成为瓶颈，否则不需要考虑使用 `ValueTask`。
>
>相关链接：
>https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/

---

## 3. `Task` 与 `ValueTask` 性能对比：

| 场景             | 耗时       | 空间   |
| -------------- | -------- | ---- |
| Task 缓存命中      | 8.72ns   | 72B  |
| ValueTask 缓存命中 | 9.76ns   | 0B   |
| Task 未命中       | 553.7ns  | 136B |
| ValueTask 未命中  | 559.32ns | 144B |

---

## 4. `ValueTask` 的局限性

### 4.1 `ValueTask` 不可被多次 `await`

> 原理：`ValueTask` 底层会使用一个对象存储异步操作的状态，而它在被 `await` 后（可以认为此时异步操作已经结束），这个对象可能已经被回收，甚至有可能已经被用在别处（或者说，`ValueTask` 可能会从已完成状态变成未完成状态）。

### 4.2 `ValueTask` 不可跨线程 `await`

> 原理：`ValueTask` 在设计之初就只是用来解决 `Task` 在个别情况下的开销问题，而不是打算全面取代 `Task`，因此`ValueTask` 并没有引入线程安全等机制，跨线程 `await ValueTask` 是线程不安全行为

### 4.3 不要阻塞调用 `ValueTask`，如 `.Result` 或 `.GetAwaiter().GetResult()`

> 原理：`ValueTask` 并不能保证阻塞当前线程，因此除非你通过判断 `IsCompleted` 等属性确定 `ValueTask` 已经完成，否则不应使用 `.Result` 等阻塞方式获取结果

---

## 5. 补充说明

1. 绝大多数情况下，直接使用 `await` 获取 `ValueTask` 的结果，而不要同步调用。
2. 另外，`ValueTask` 还具有 `AsTask()` 方法将其转为传统的`Task`，以进行常规操作（那为什么不直接使用 `Task`？）。

---

## 6. 结语

- `ValueTask` 适用场景：你已知将会有大量“假异步”，使用 `ValueTask` 能明显优化性能之时。
-  `ValueTask` 是一个非常有用的工具，可以在特定场景下显著提升性能，尤其是在高频调用且缓存命中率较高的情况下。
- 然而，它并非`Task` 的优化版本，其本身具有比 `Task` 更多的限制。因此对于大多数应用程序来说，只有在性能分析表明 Task 的开销成为瓶颈时，才考虑使用 `ValueTask`。

---

## 7. 参考链接

- [如何正确使用C#中的ValueTask？](https://www.bilibili.com/video/BV1dm421j72Y)
- [博客文章](https://blog.coldwind.top/posts/why-we-need-valuetask/)
- [.Net文档](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/)
- [youtube视频](https://www.youtube.com/watch?v=dCj7-KvaIJ0)