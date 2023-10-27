/*object obj = new();

lock (obj)
{
    Console.WriteLine("do somethig...");
}*/



object obj = new();
//是否获取锁
bool lockTaken = false;
try
{
    //尝试获取锁，成功则修改 lockTaken，然后执行下面的任务
    Monitor.Enter(obj, ref lockTaken);
    Console.WriteLine("do somethig...");
}
finally
{
    //刚才获取锁成功了，任务结束后释放锁
    if (lockTaken)
    {
        Monitor.Exit(obj);
    }
}

