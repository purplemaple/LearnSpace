using _2._0_ChainOfResponsibilityOptimize.Handlers;

namespace _2._0_ChainOfResponsibilityOptimize
{
    internal class Program
    {
        /*
         * 责任链设计模式优化
         * 使用继承关系将案例 1.0 进行优化
         */
        static void Main(string[] args)
        {
            Manager commonManager = new CommonManager("经理");
            Manager majorManager = new MajorManager("总监");
            Manager generalManager = new GeneralManager("总经理");

            //设置总监为经理的上级，总经理为总监的上级
            commonManager.SetSuperior(majorManager);
            majorManager.SetSuperior(generalManager);

            Request request1 = new Request()
            {
                RequestType = "请假",
                RequestContent = "小菜请假",
                Count = 1
            };
            commonManager.RequestApplications(request1);

            Request request2 = new Request()
            {
                RequestType = "请假",
                RequestContent = "小菜请假",
                Count = 4
            };
            commonManager.RequestApplications(request2);

            Request request3 = new Request()
            {
                RequestType = "加薪",
                RequestContent = "小菜加薪",
                Count = 500
            };
            commonManager.RequestApplications(request3);

            Request request4 = new Request()
            {
                RequestType = "加薪",
                RequestContent = "小菜加薪",
                Count = 1000
            };
            commonManager.RequestApplications(request4);
        }
    }
}