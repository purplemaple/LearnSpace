namespace _1_GeneralSyncMethod
{
    //1. 对于普通的同步方法，可以使用 CancellationToken 来进行取消
    //因为 CancellationToken 处于 System.Thread 命名空间，而非 System.Thread.Task 中，所以设计之初就并非异步方法才能使用
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        void Foo(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            while (true)
            {
                if (token.IsCancellationRequested)
                    return;

                // DO some heavy work
            }
        }
    }
}
