using _3._2_LINQForDataPage.Model;
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

namespace _3._2_LINQForDataPage.ViewModel
{
    /*
     * 注：本项目修改了 DataGrid 的 ItemSource 绑定逻辑，从之前的 ICollectionView CollectionView 改成了 IEnumerable<Employee> EmployeeDisplay
     */
    public class MyViewModel : BindableBase
    {
        List<Employee> employees;

        //这里用 List 也是可以的
        private IEnumerable<Employee> employeeDisplay;

        public IEnumerable<Employee> EmployeeDisplay
        {
            get => employeeDisplay;
            set => SetProperty(ref employeeDisplay, value);
        }

        //每页显示的元素个数，暂定为15
        public const int PageSize = 15;

        private int pageNumber;

        public MyViewModel(IEnumerable<Employee> employees)
        {
            //用 employees 暂存所有信息
            this.employees = new(employees);

            //第一次刷新，直接使用 Take 拿取前 PageSize 条数据即可
            EmployeeDisplay = this.employees.Take(PageSize);
        }

        public DelegateCommand<string> GotoPageCommand => new DelegateCommand<string>((pageNumber) =>
        {
            this.pageNumber = Convert.ToInt32(pageNumber);

            /*
             * 使用 Skip() 跳过前 xx 条数据，然后使用 Take() 获取指定条数的数据
             * 时间复杂度：O(1)
             * 
             * 注：Skip() 和 Take() 的时间复杂度区别于操作的集合类型，
             *      如果集合类型是 IList，则都为 O(1)
             *      如果集合类型是 IEnumerable，则都为 O(n)，(这里 Skip() 和 Take() 是线性拼接而非嵌套，因此复杂度按加法算即 n + n = 2n)，因此总复杂度为 O(n)
             *      
             * 本项目中，employees 是实现了 IList 的 List 类型，因此复杂度为 O(1)
             * 
             * 补充：
             *      1. 实现了 IList 的集合可以通过索引直接访问元素，所以不需要遍历整个集合，时间复杂度为 O(1)
             *      2. 而 IEnumerable 类型的集合需要使用迭代器访问元素，因此即使获取指定数量的元素也需要遍历集合，时间复杂度 O(n)
             */
            EmployeeDisplay = employees.Skip(PageSize * (this.pageNumber - 1)).Take(PageSize);
        });

    }
}
