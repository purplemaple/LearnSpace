namespace _4_ThirdPartyClassLib;

//4.1 假设这是一个第三方库，我们无法修改它的代码
public class ThirdPartyUtils
{
    //中间有阻塞点的线程，可以被打断
    public static bool CancelableSyncMethod()
    {
        for (int i = 0; i < 100; i++)
        {
            Thread.Sleep(100);
        }

        Console.WriteLine("Third-party cancelable job done.");
        return true;
    }

    //不具备任何阻塞点的计算密集型线程，将不会被 Thread.Interrupt() 打断
    public static bool UnCancelableSyncMethod()
    {
        long sum = 0;
        for (long i = 0; i < 10_000_000_000; i++)
        {
            sum++;
        }

        Console.WriteLine("Third-party uncancelable job done.");
        return true;
    }
}
