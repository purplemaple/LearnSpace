using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4._0_CommandPatternOptimize
{
    /// <summary>
    /// 服务员类
    /// </summary>
    public class Waiter
    {
        private IList<Command> orders = new List<Command>();

        //设置订单
        public void SetOrder(Command command)
        {
            //这里这样写是因为客户端传入时，是：waiter.SetOrder(bakeChickWingCommand1); 然后 bakeChickWingCommand1.ToString() 就是它的命名空间 + 类名
            if (command.ToString() == "_4._0_CommandPatternOptimize.BakeChickenWingCommand") 
            {
                Console.WriteLine("服务员：鸡翅没有了，清点别的烧烤。");
            }
            else
            {
                orders.Add(command);
                Console.WriteLine("增加订单：" + command.ToString() + " 时间：" + DateTime.Now.ToString());
            }
        }

        //取消订单
        public void CancelOrder(Command command)
        {
            orders.Remove(command);
            Console.WriteLine("取消订单：" + command.ToString() + " 时间：" + DateTime.Now.ToString());
        }

        //通知全部执行
        public void Notify()
        {
            foreach(Command cmd in  orders)
            {
                cmd.ExecuteCommand();
            }
        }
    }
}
