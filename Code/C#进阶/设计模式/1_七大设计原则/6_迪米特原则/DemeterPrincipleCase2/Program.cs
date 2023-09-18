namespace DemeterPrincipleCase2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //打印总公司员工的ID和分公司员工的ID

            /*
             * 初始版本流程
             * 类1: 总公司员工类
             * 类2: 总公司员工管理类
             *      1. 获取总公司所有员工
             *      2. 打印总公司所有员工ID
             *      3. 打印分公司所有员工ID
             * 
             * 类3: 分公司员工类
             * 类4: 分公司员工管理类
             *      1. 获取分公司所有员工
             */


            HeadOfficeManager headOfficeManager = new HeadOfficeManager();
            BranchOfficeManager branchOfficeManager = new BranchOfficeManager();
            headOfficeManager.Print(branchOfficeManager);
        }

        class HeadOfficeEmployee
        {
            public int ID { get; set; }
        }

        class BranchOfficeEmployee
        {
            public int ID { get; set; }
        }


        /*
         * 分析: 对于HeadOfficeManager而言，谁是他的直接朋友?
         * 1. HeadOfficeEmployee: 出现在方法返回值中 --> 是直接朋友
         * 2. BranchOfficeManager: 出现在方法参数中 --> 是直接朋友
         * 3. BranchOfficeEmployee: 通过局部变量出现在本类中 --> 不是直接朋友
         */
        class HeadOfficeManager
        {
            //获取总公司所有员工
            public List<HeadOfficeEmployee> GetHeadOfficeEmployees()
            {
                List<HeadOfficeEmployee> list = new List<HeadOfficeEmployee>();
                for (int i = 0; i < 10; i++)
                {
                    HeadOfficeEmployee headOfficeEmployee = new HeadOfficeEmployee();
                    headOfficeEmployee.ID = i;
                    list.Add(headOfficeEmployee);
                }
                return list;
            }

            //打印总公司员工和分公司所有员工的ID
            public void Print(BranchOfficeManager branchOfficeManager)
            {
                //1. 获取总公司所有员工
                List<HeadOfficeEmployee> listHead = this.GetHeadOfficeEmployees();
                Console.WriteLine("总公司所有员工的ID");
                foreach (HeadOfficeEmployee employee in listHead)
                {
                    Console.WriteLine(employee.ID);
                }

                //2. 打印分公司所有员工的ID
                //注: listBranch 这个集合对象, 是通过局部变量的形式出现在本类中，所以不是直接朋友
                List<BranchOfficeEmployee> listBranch = branchOfficeManager.GetBranchOfficeEmployees();
                Console.WriteLine("分公司所有员工的ID");
                foreach (BranchOfficeEmployee employee in listBranch)
                {
                    Console.WriteLine(employee.ID);
                }
            }
        }

        class BranchOfficeManager
        {
            //获取分公司员工
            public List<BranchOfficeEmployee> GetBranchOfficeEmployees()
            {
                List<BranchOfficeEmployee> list = new List<BranchOfficeEmployee>();
                for (int i = 0; i < 5; i++)
                {
                    BranchOfficeEmployee branchOfficeEmployee = new BranchOfficeEmployee();
                    branchOfficeEmployee.ID = i;
                    list.Add(branchOfficeEmployee);
                }
                return list;
            }
        }
    }
}