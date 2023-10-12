using GenericityReflectionFactory.Model;
using GenericityReflectionFactory.Service;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using static GenericityReflectionFactory.MyEnums;

namespace GenericityReflectionFactory
{
    internal class Program
    {
        /*
         * 实战案例
         * 
         */
        static void Main(string[] args)
        {
            Enum modelType = ModelType.Model_1;
            double x = 3;
            double y = 7;

            //获取要操作的 Model 对象
            TReflectionFactory<BaseModel> modelFactory = new();
            BaseModel model = modelFactory.Create(modelType);

            //根据 Model 对象获取指定的计算实现类
            TReflectionFactory<ICalcable> calcFactory = new();
            ICalcable calcService = calcFactory.Create(modelType, model);

            Console.WriteLine("-----------------------------");

            Console.WriteLine("本次计算的参数为：" + x + "，" + y);
            Console.WriteLine("结果为：" + calcService.Calc(x, y));

            Console.WriteLine("-----------------------------");

        }
    }
}