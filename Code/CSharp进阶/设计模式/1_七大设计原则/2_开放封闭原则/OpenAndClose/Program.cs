namespace OpenAndClose
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
                        bankProcess.Deposite();
                        break;
                    case "取款":
                        bankProcess.DrawMoney();
                        break;
                    case "转账":
                        bankProcess.Transfer();
                        break;
                    default:
                        Console.WriteLine("目前没有办法处理您的业务~~");
                        break;
                }
            }
        }

        /// <summary>
        /// 不符合单一职责原则
        /// </summary>
        public class BankProcess
        {
            //存钱
            public void Deposite()
            {
                Console.WriteLine("处理用户的存款");
            }
            //取钱
            public void DrawMoney()
            {
                Console.WriteLine("处理用户的取款");
            }
            //转账
            public void Transfer()
            {
                Console.WriteLine("处理用户的转账");
            }
        }

    }
}