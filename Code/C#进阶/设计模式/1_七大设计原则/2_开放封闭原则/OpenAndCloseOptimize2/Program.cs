using static OpenAndCloseOptimize2.Program;

namespace OpenAndCloseOptimize2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //在类中，将每一个方法都进行接口抽象也比较极端。所以根据实际的业务情况减少接口的封装

            BankClient bankClient = new BankClient();
            bankClient.BankType = "抢劫";
            BankStuff bankStuff = new BankStuff();
            bankStuff.HandleProcess(bankClient);
        }

        public class BankClient
        {
            public string BankType { get; set; }
        }

        public class BankStuff
        {
            //private BankProcess bankProcess = new BankProcess();

            //拿到接口引用
            private IBankProcess _bankProcess;
            public void HandleProcess(BankClient bankClient)
            {
                //调用银行的业务系统，处理用户的请求
                switch (bankClient.BankType)
                {
                    case "存款":
                        _bankProcess = new DepositeClass();
                        _bankProcess.BankProcess();
                        break;
                    case "取款":
                        _bankProcess = new DrawMoneyClass();
                        _bankProcess.BankProcess();
                        break;
                    case "转账":
                        _bankProcess = new TransferClass();
                        _bankProcess.BankProcess();
                        break;
                    default:
                        Console.WriteLine("目前没有办法处理您的业务~~");
                        break;
                }
            }
        }


        public interface IBankProcess
        {
            void BankProcess();
        }

        public class DepositeClass : IBankProcess
        {
            public void BankProcess()
            {
                Console.WriteLine("存款");
            }
        }

        public class DrawMoneyClass : IBankProcess
        {
            public void BankProcess()
            {
                Console.WriteLine("取款");
            }
        }

        public class TransferClass : IBankProcess
        {
            public void BankProcess()
            {
                Console.WriteLine("转账");
            }
        }
    }
}