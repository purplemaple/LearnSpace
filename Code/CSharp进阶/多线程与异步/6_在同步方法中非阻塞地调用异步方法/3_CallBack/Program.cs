namespace _3_CallBack
{

    //3. 使用 CallBack 安全地一发即忘
    // 无论是使用普通方法还是扩展方法，本质还是 async void，程序不会崩溃，不过外层线程仍然无法捕获异常
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //类的构造函数中会调用异步方法，主线程不能阻塞等待，否则太耗时间了
                MyDataModel dataModel = new();

                Console.WriteLine("Loading data...");
                Thread.Sleep(2000);
                var data = dataModel.Data;
                Console.WriteLine($"Data is loaded: {dataModel.IsDataLoaded}");
            }
            catch (Exception ex)
            {
                //注：使用回调在异步任务的线程处理异常时，主线程仍然无法捕获异常，但程序起码不会崩溃
                Console.WriteLine(ex);
            }
        }
    }

    class MyDataModel
    {
        public List<int>? Data { get; private set; }

        public bool IsDataLoaded { get; private set; } = false;

        public MyDataModel()
        {
            //SafeFireAndForget(LoadDataAsync(), () => IsDataLoaded = true, e => throw e);

            LoadDataAsync().SafeAwait(() => IsDataLoaded = true, e => throw e);
        }

        //3.1 使用回调方法对一发即忘进行包装，正确地处理异常
        static async void SafeFireAndForget(Task task, Action? onCompleted = null, Action<Exception>? onError = null)
        {
            //这里处于异步线程，在这正确处理异常可以避免程序崩溃
            try
            {
                await task;
                onCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            } 
        }

        /// <summary>
        /// Mimic loading data from database
        /// </summary>
        /// <returns></returns>
        async Task LoadDataAsync()
        {
            await Task.Delay(1000);
            Data = Enumerable.Range(1, 10).ToList();

            throw new Exception("Failed to load data.");
        }
    }

    //3.2 将回调方法包装为 Task 类的扩展

    static class TaskExtensions
    {
        public static async void SafeAwait(this Task task, Action? onCompleted = null, Action<Exception>? onError = null)
        {
            try
            {
                await task;
                onCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }
    }
}
