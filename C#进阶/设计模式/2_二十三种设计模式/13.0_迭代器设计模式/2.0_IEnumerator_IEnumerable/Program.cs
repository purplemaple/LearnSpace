/*
 * 本项目用于演示 .NET 框架中已准备好的迭代器接口
 */

IList<object> aggregate = new List<object>
{
    "a",
    "b",
    'c',
    new int[]{1,3,6,9,4},
    1.2
};

foreach(var item in aggregate)
{
    Console.WriteLine("集合内元素：" + item);
}

/*
 * foreach in 的原理：
 * 执行过程：
 * 
 * IEnumerator<object> e = aggregate.GetEnumerator();
 * 
 * while(e. MoveNext())
 * {
 *     return e.Current;
 * }
 * 
 * 注：IList 实现了 IEnumerable 接口，而这个接口提供了 GetEnumerator() 这个获取迭代器的方法
 * 由此可知，迭代功能是由 IEnumerator 和 IEnumerable 两个接口共同支持
 */