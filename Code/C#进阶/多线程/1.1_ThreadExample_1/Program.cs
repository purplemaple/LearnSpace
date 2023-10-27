const int total = 100_000;

int count = 0;
//简单互斥锁(若需要多进程间的互斥锁请看 Mutex)
object lockObj = new();

Thread t1 = new(ThreadMethod);
Thread t2 = new(ThreadMethod);


t1.Start();
t2.Start();

//让主线程等待两个子线程完成后再打印
t1.Join();
t2.Join();

Console.WriteLine("Count：" + count);

void ThreadMethod()
{
    for (int i = 0; i < total; i++)
        Interlocked.Increment(ref count);
        /*lock(lockObj)
            count++;*/
}

