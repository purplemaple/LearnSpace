using _4_ThirdPartyClassLib;

namespace _4_ThirdPartyClassLibWrapper;

//4.2 对第三方库进行包装，会编译为 exe 文件，主程序通过这个 exe 文件来调用第三方库的功能，需要取消时直接杀死这个 exe 进程
internal class Program
{
    public static void Main(string[] args)
    {
        ////首先测试一下 Main 函数的入参
        //for (int i = 0; i < args.Length; i++)
        //{
        //    Console.WriteLine($"Argument {i}: {args[i]}");
        //}

        //这是非常简陋的写法，仅供演示
        if (args[0] == "cancelable")
            ThirdPartyUtils.CancelableSyncMethod();
        else if (args[0] == "uncancelable")
            ThirdPartyUtils.UnCancelableSyncMethod();
    }
}
