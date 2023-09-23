using _3._1_FilterForDataPage.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _3._1_FilterForDataPage.ViewModel
{
    /*
     * 本项目在 1.0 和 2.0 的基础上演示如何实现 DataGrid 的分页功能
     * 1. 想到在 1.0 项目中，可以用过滤器 Filter 控制 DataGrid 显示的内容，当然也可以拿来做分页功能
     *      缺陷：所有数据过一遍过滤器，过滤器内还要获取数据下标，组合起来时间复杂度O(n^2)(即使像下面例子，使用 Dictionary 记录元素下标，但当遇到元素增减时，下标打乱又得重新计算，时间复杂度还是很高)
     * 2. 使用 LINQ 的 Skip() 以及 Take() 操作数据集合实现分页与跳转
     * 3. 在 3.2 项目的基础上，实现当用户删除 dataGrid 中某一行时，依旧能正常分页与跳转
     */
    public class MyViewModel : BindableBase
    {
        private List<Employee> employees;

        public List<Employee> Employees
        {
            get => employees;
            set => SetProperty(ref employees, value);
        }

        private ICollectionView collectionView;

        public ICollectionView CollectionView
        {
            get => collectionView;
            set => SetProperty(ref collectionView, value);
        }

        private string filterText;

        public string FilterText
        {
            get => filterText;
            set
            {
                SetProperty(ref filterText, value);
                /*
                 * 想要在 FilterText 值发生改变时执行逻辑，直接写在这里就行了，不需要写什么触发器(Trigger)
                 */
                CollectionView.Refresh();           //刷新 CollectionView，因为在构造中已经写好了过滤器
            }
        }

        //跳转到的页数
        private int pageNumber;

        //每页显示的元素个数，暂定为15
        public const int PageSize = 15;

        //使用 Dictionary 记录集合内元素的下标，防止每次查找时都重复获取下标导致的 O(n^2) 复杂度
        Dictionary<Employee, int> indexCache = new();

        public MyViewModel(IEnumerable<Employee> employees)
        {
            Employees = new List<Employee>(employees);

            //使用 Dictionary 记录集合内元素的下标，防止每次查找时都重复获取下标导致的 O(n^2) 复杂度
            for (int i = 0;i < this.employees.Count;i++)
            {
                indexCache[this.employees[i]] = i;
            }

            CollectionView = CollectionViewSource.GetDefaultView(Employees);
            CollectionView.Filter = (item) =>
            {
                //这种写法的缺点：在 Filter 中，每一个 item 都要执行一次这个 IndexOf()，因此时间复杂度为 O(n^2) 
                //var index = this.employees.IndexOf((Employee)item); //IndexOf()：获取集合内元素的下标

                var index = indexCache[(Employee)item];
                return pageNumber == index / PageSize + 1;
            };

            GotoPageCommand.Execute("1");
        }

        /*
         * 注：通过前台 CommandParameter="{Binding ElementName=dataGrid, Path=SelectedItems}" 命令传递过来的集合参数是 IList 类型
         */
        public DelegateCommand<IList> CalculateSumSalaryCommand => new DelegateCommand<IList>((employees) =>
        {
            //1. 基础写法
            /*double sum = 0;
            foreach (var e in employees)
            {
                sum += ((Employee)e).Salary;
            }*/

            //2. 进阶写法
            /*
             * Cast<TResult>()：尝试将源集合的每个元素转换成 TResult 类型，若失败则抛 InvalidCastException
             * ToList()：是为了转成集合后方便使用 LINQ 简化步骤
             * Sum()：对集合中元素求和，可用于任何实现了 IEnumerable<T> 接口的类型，如数组、列表、集合等
             */
            double sum = employees.Cast<Employee>().ToList().Sum(x => x.Salary);
        });


        public DelegateCommand<string> GotoPageCommand => new DelegateCommand<string>((pageNumber) =>
        {
            //接收前台要跳转的页数
            this.pageNumber = Convert.ToInt32( pageNumber);
            //刷新 CollectionView (分页在过滤器里设置了)
            CollectionView.Refresh();
        });

    }
}
