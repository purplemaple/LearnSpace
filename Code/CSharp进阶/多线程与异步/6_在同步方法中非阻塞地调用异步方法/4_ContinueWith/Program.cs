namespace _4_ContinueWith
{
    //4. 使用 ContinueWith 解决一发即忘的问题 -> 使用 ContinueWith 时最好设置一下 TaskScheduler
    //ContinueWith 本质是 async Task，可以解决异常被吞的问题
    //缺点：
    //  1. ContinueWith 一定会将传入的委托包装成一个 Task，即便该委托不是异步任务，这样会导致额外消耗
    //  2. ContinueWith 的 TaskScheduler(控制后续任务在哪个线程上完成) 的默认值为 TaskScheduler.Current，而非 TaskScheduler.Default，这样可能会有死锁风险

    // 扩展：ContinueWith 方法实现单元测试：https://www.youtube.com/watch?v=vYXs--S0Xxo
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
            //ContinueWith 只能传入一个回调委托，因此这里将需要做的事全都包装在一个委托内
            LoadDataAsync().ContinueWith(OnDataLoaded, TaskScheduler.Default);
        }

        private bool OnDataLoaded(Task task)
        {
            if (task.IsFaulted)
            {
                //这里使用 InnerException 拿到内部异常，否则拿到的是 AggregateException
                Console.WriteLine(task.Exception.InnerException.Message);
            }
            return IsDataLoaded = true;
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
}
