using _3._3_DataPageOnChanged.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _3._3_DataPageOnChanged.ViewModel
{
    /*
     * 注：本项目修改了 3.2 中 EmployeeDisplay 的类型，从之前的 IEnumerable 改成了 ObservableCollection
     */
    public class MyViewModel : BindableBase
    {
        List<Employee> employees;

        //这里要实现增删逻辑，因此改为 ObservableCollection
        private ObservableCollection<Employee> employeeDisplay;

        public ObservableCollection<Employee> EmployeeDisplay
        {
            get => employeeDisplay;
            set => SetProperty(ref employeeDisplay, value);
        }

        private int pageNumber = 1;

        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if(value < 1) value = 1;
                SetProperty(ref pageNumber, value);
            }
        }

        //每页显示的元素个数，暂定为15
        public const int PageSize = 15;


        public MyViewModel(IEnumerable<Employee> employees)
        {
            //用 employees 暂存所有信息
            this.employees = new(employees);

            //第一次刷新，直接使用 Take 拿取前 PageSize 条数据即可
            //EmployeeDisplay = new ObservableCollection<Employee>(this.employees.Take(PageSize));
            EmployeeDisplay = new(this.employees.Take(PageSize));   //这里是简写，完整如上行，不赘述

            //给 EmployeeDisplay 发生改变的事件(即前台发生增删时)挂载事件处理器
            EmployeeDisplay.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            /*
             * 注：前台绑定的数据源是 EmployeeDisplay，而分页的数据源是 employees，因此前台删除时，要同步更新 employees
             * TODO: 这里指管了删除暂未管增加
             */
            employees.Remove((Employee)e.OldItems[0]);
            GotoPage();
        }

        public DelegateCommand GotoPageCommand => new DelegateCommand(GotoPage);

        private void GotoPage()
        {
            var disPlays = employees.Skip(PageSize * (this.PageNumber - 1)).Take(PageSize);

            //这里是为了实现，当本页元素全都删完时，能智能地跳转到前一页
            if(disPlays.Count() == 0 && pageNumber > 1)
            {
                //pageNumber - 1 后递归调自己即可
                PageNumber -= 1;
                GotoPage();
                return;
            }

            EmployeeDisplay = new(disPlays);
            //因为上面每次 new 完，事件处理器都会丢失，因此要重新挂载
            EmployeeDisplay.CollectionChanged += CollectionChanged;
        }

    }
}
