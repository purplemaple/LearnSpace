using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1._0_Iterator.IteratorService;

namespace _1._0_Iterator.AggregateService
{
    /// <summary>
    /// 聚集实现类
    /// </summary>
    class ConcreteAggregate : Aggregate
    {
        //用于装聚集内元素的集合，由于是 object 泛型，因此可以混杂装各种类型
        private IList<object> items = new List<object>();
        public override Iterator CreateIterator()
        {
            return new ConcreteIterator(this);
        }

        //返回聚集的总个数
        public int Count
        {
            get => items.Count;
        }

        //这样写就能像数组一样用下标访问或添加了
        public object this[int index]
        {
            get => items[index];
            set => items.Insert(index, value);
        }
    }
}
