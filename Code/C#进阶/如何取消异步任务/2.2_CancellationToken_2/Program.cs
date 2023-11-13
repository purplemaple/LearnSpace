class Demo
{
    Task FooAsync3(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            //1. 在任务开始前先判断任务是否已被取消，是则抛出异常
            if(cancellationToken.IsCancellationRequested)
                //ThrowIfCancellationRequested() 是自带的方法，可以抛出一个 OperationCanceledException 异常
                cancellationToken.ThrowIfCancellationRequested();

            //模拟一个轮询操作
            while(true)   
            {
                /*
                 * 2. 轮询任务内也每次都判断一下任务是否被取消
                 * 注：这里是假设 while 有其他的判断条件，才在内部做判断
                 * 否则可以直接当成循环条件写，如：
                 * while(!IsCancellationRequested)
                 */
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                Thread.Sleep(1000);
                Console.WriteLine("Pooling...");
            }
        });
    }
}