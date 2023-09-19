using _1._0_DataFilter.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _1._0_DataFilter.ViewModel
{
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
    }
}
