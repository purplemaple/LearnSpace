using _1._0_Iterator.AggregateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_Iterator.IteratorService
{
    /// <summary>
    /// 倒序迭代的迭代器
    /// </summary>
    class ConcreteIteratorDesc : Iterator
    {
        public ConcreteIteratorDesc(ConcreteAggregate aggregate)
        {
            this.aggregate = aggregate;
            //将初始位置移动到最后一位
            current = aggregate.Count - 1;
        }
        public override object First()
        {
            return aggregate[aggregate.Count - 1];
        }

        public override object Next()
        {
            object ret = null;
            current--;
            if (current >= 0)
            {
                ret = aggregate[current];
            }

            return ret;
        }

        public override bool IsDone()
        {
            return current < 0;
        }

        public override object CurrentItem()
        {
            return aggregate[current];
        }
    }
}
