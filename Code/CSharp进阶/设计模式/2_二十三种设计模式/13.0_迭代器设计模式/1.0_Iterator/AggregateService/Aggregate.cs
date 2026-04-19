using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1._0_Iterator.IteratorService;

namespace _1._0_Iterator.AggregateService
{
    /// <summary>
    /// 聚集抽象类
    /// </summary>
    abstract class Aggregate
    {
        //创建迭代器
        public abstract Iterator CreateIterator();
    }
}
