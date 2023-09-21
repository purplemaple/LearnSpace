using _3._1_FilterForDataPage.Model;
using _3._1_FilterForDataPage.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _3._1_FilterForDataPage.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Employee> employees = new List<Employee>()
            {
                new Employee(1, "张", "三", "2023-9-1", 1000),
                new Employee(2, "李", "四", "2023-9-2", 2500),
                new Employee(3, "张", "四", "2023-5-23", 3000),
                new Employee(4, "王", "小六", "2023-6-13", 1500),
                new Employee(5, "赵", "二狗", "2023-2-31", 12000),
                new Employee(6, "张", "飞", "2023-5-12", 2000),
                new Employee(7, "王", "琪", "2023-7-7", 1200),
                new Employee(8, "周", "国庆", "2023-9-12", 2400),
                new Employee(9, "郑", "德柱", "2023-2-5", 1600),
                new Employee(10, "张", "三", "2023-9-1", 1000),
                new Employee(11, "李", "四", "2023-9-2", 2500),
                new Employee(12, "张", "四", "2023-5-23", 3000),
                new Employee(13, "王", "小六", "2023-6-13", 1500),
                new Employee(14, "赵", "二狗", "2023-2-31", 12000),
                new Employee(15, "张", "飞", "2023-5-12", 2000),
                new Employee(16, "王", "琪", "2023-7-7", 1200),
                new Employee(17, "周", "国庆", "2023-9-12", 2400),
                new Employee(18, "郑", "德柱", "2023-2-5", 1600),
                new Employee(19, "张", "三", "2023-9-1", 1000),
                new Employee(20, "李", "四", "2023-9-2", 2500),
                new Employee(21, "张", "四", "2023-5-23", 3000),
                new Employee(22, "王", "小六", "2023-6-13", 1500),
                new Employee(23, "赵", "二狗", "2023-2-31", 12000),
                new Employee(24, "张", "飞", "2023-5-12", 2000),
                new Employee(25, "王", "琪", "2023-7-7", 1200),
                new Employee(26, "周", "国庆", "2023-9-12", 2400),
                new Employee(27, "郑", "德柱", "2023-2-5", 1600),
                new Employee(28, "张", "三", "2023-9-1", 1000),
                new Employee(29, "李", "四", "2023-9-2", 2500),
                new Employee(30, "张", "四", "2023-5-23", 3000),
                new Employee(31, "王", "小六", "2023-6-13", 1500),
                new Employee(32, "赵", "二狗", "2023-2-31", 12000),
                new Employee(33, "张", "飞", "2023-5-12", 2000),
                new Employee(34, "王", "琪", "2023-7-7", 1200),
                new Employee(35, "周", "国庆", "2023-9-12", 2400),
                new Employee(36, "郑", "德柱", "2023-2-5", 1600),
            };
            this.DataContext = new MyViewModel(employees);
        }
    }
}
