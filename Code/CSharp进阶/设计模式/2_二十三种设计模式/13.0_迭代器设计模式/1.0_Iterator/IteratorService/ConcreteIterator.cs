using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1._0_Iterator.AggregateService;

namespace _1._0_Iterator.IteratorService
{
    /// <summary>
    /// 迭代器实现类<br/>
    /// 内含一个具体的聚集对象，根据这个聚集对象，生成专属的迭代器
    /// </summary>
    class ConcreteIterator : Iterator
    {

        //构造时需传入一个具体的聚集对象，这样，这个迭代器就是这个聚集对象的专属迭代器
        public ConcreteIterator(ConcreteAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        //返回第一个聚集对象
        public override object First()
        {
            return aggregate[0];
        }

        //先往后走一位，然后判断此位置是否已越界，否则返回此位置的聚集对象
        public override object Next()
        {
            object ret = null;
            current++;
            if (current < aggregate.Count)
            {
                ret = aggregate[current];
            }
            return ret;
        }

        //判断是否已越界(已遍历完整个聚集对象)
        public override bool IsDone()
        {
            return current >= aggregate.Count;
        }

        //返回当前位置的聚集对象
        public override object CurrentItem()
        {
            return aggregate[current];
        }
    }
}
