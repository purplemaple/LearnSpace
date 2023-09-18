namespace OpenAndCloseOptimize
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
            private BankProcess bankProcess = new BankProcess();
            public void HandleProcess(BankClient bankClient)
            {
                //调用银行的业务系统，处理用户的请求
                switch (bankClient.BankType)
                {
                    case "存款":
                        bankProcess.Deposite = new DepositeClass();
                        bankProcess.DepositeFuc();
                        break;
                    case "取款":
                        bankProcess.DrawMoney = new DrawMoneyClass();
                        bankProcess.DrawMoneyFuc();
                        break;
                    case "转账":
                        bankProcess.Transfer = new TransferClass();
                        bankProcess.TransferFuc();
                        break;
                    default:
                        Console.WriteLine("目前没有办法处理您的业务~~");
                        break;
                }
            }
        }

       
        public class BankProcess
        {
            public IDeposite Deposite { get; set; }
            public IDrawMoney DrawMoney { get; set; }
            public ITransfer Transfer { get; set; }

            public void DepositeFuc()
            {
                this.Deposite.DepositeInterface();
            }

            public void DrawMoneyFuc()
            {
                this.DrawMoney.DrawMoneyInterface();
            }

            public void TransferFuc()
            {
                this.Transfer.TransferInterface();
            }
        }

        //1. 抽取接口
        //2. 在BankProcess中进行调用

        public interface IDeposite
        {
            void DepositeInterface();
        }
        public interface IDrawMoney
        {
            void DrawMoneyInterface();
        }
        public interface ITransfer
        {
            void TransferInterface();
        }
        public class DepositeClass : IDeposite
        {
            public void DepositeInterface()
            {
                Console.WriteLine("存款");
            }
        }
        public class DrawMoneyClass : IDrawMoney
        {
            public void DrawMoneyInterface()
            {
                Console.WriteLine("取款");
            }
        }
        public class TransferClass : ITransfer
        {
            public void TransferInterface()
            {
                Console.WriteLine("转账");
            }
        }

        
    }
}