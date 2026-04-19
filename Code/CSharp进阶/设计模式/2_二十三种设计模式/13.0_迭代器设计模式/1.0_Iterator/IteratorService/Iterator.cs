using _1._0_Iterator.AggregateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_Iterator.IteratorService
{
    /// <summary>
    /// 迭代器抽象类<br/>
    /// </summary>
    abstract class Iterator
    {
        //具体的聚集对象(到时候生成的迭代器就是这个对象的专属迭代器)
        public ConcreteAggregate aggregate;
        //当前位置记录器
        public int current = 0;

        public abstract object First();
        public abstract object Next();
        public abstract bool IsDone();
        public abstract object CurrentItem();
    }
}
