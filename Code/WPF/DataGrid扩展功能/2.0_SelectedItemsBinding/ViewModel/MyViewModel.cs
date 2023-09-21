using _2._0_SelectedItemsBinding.Model;
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

namespace _2._0_SelectedItemsBinding.ViewModel
{
    /*
     * 本项目在 1.0 DataFilter(数据筛选)项目的基础上，再演示 DataGrid 中的 SelectedItems(注意是复数，表示多选项) 如何绑定到 ViewModel 上
     * 1. 不绑定，而是使用 CommandParameter，直接将 SelectedItems 属性当参数传到后台命令上(注意传过来的是 IList 类型)
     * 2. 使用 Behavior - 附加属性法
     * 3. 使用依赖属性法
     * 4. 使用 i:EventTrigger 法(违背了 MVVVM 模式)
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

        public MyViewModel(IEnumerable<Employee> employees)
        {
            //IEnumerable 对象不好直接操作，因此这里转换一下，下面第一种可能为null，第二种好一点
            //Employees = employees as List<Employee>;
            Employees = new List<Employee>(employees);

            CollectionView = CollectionViewSource.GetDefaultView(Employees);
            CollectionView.Filter = (item) =>
            {
                //如果文本为 null 或为空格的话不筛选
                if (string.IsNullOrWhiteSpace(FilterText)) return true;
                var em = item as Employee;
                return em.FirstName.Contains(FilterText) || em.LastName.Contains(FilterText) || (em.FirstName + em.LastName).Contains(FilterText);
            };
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

    }

    /*
     * 补充：Sum() 的其他使用场景：
     * 1. 对一组数字进行求和，如：int sum = Sum(1, 2, 3, 4); // sum = 10
     * 2. 对一组字符串进行拼接，如：string concat = Sum("a", "b", "c", "d"); // concat = "abcd"
     * 3. 对一组布尔值进行 '或' 运算，如：bool result = Sum(true, false, true, false); // result = true
     */
}
