using _1._0_Iterator.AggregateService;
using _1._0_Iterator.IteratorService;


/*
 * 迭代器模式：提供一种方法按一定的顺序访问一个聚合内对象中各个元素，而又不暴露该对象的内部结构
 * 迭代器模式就是分离了集合对象的遍历行为，抽象出一个迭代器类来负责，这样既可以做到不暴露集合的内部结构，又可以让外部代码访问集合内部的资源
 * 
 * 适用场景：
 *      1. 当需要访问一个聚集对象，而不管聚集对象内的元素是什么，都需要遍历时，可使用迭代器设计模式
 *      2. 需要对聚集有多种遍历方式时，可使用迭代器设计模式(通过不同的迭代器实现类实现不同的遍历方式，如正序、倒序...)
 *      
 * 注：
 *      1. 由于迭代器过于频繁地被使用，现代编程语言基本已经内置在语法中了，如 foreach 循环
 *      2. 另外还有 C# 的 IEnumerable 、IEnumerator 接口也是为迭代器模式准备的，详见项目 2.0
 */

ConcreteAggregate aggregate = new();

aggregate[0] = "a";
aggregate[1] = "a";
aggregate[2] = 'c';
aggregate[3] = new int[] { 1,3,6,9,3};
aggregate[4] = "a";
aggregate[5] = 1;

Iterator iter = new ConcreteIterator(aggregate);
object item = iter.First();
Console.WriteLine("聚合内首元素：" + item);
while (!iter.IsDone())
{
    Console.WriteLine("聚合内元素：" + iter.CurrentItem());
    iter.Next();
}

Console.WriteLine("-----------------------");
Iterator iterDesc = new ConcreteIteratorDesc(aggregate);
object itemDesc = iterDesc.First();
Console.WriteLine("聚合内末元素：" + itemDesc);
while (!iterDesc.IsDone())
{
    Console.WriteLine("聚合内元素倒序排列：" + iterDesc.CurrentItem());
    iterDesc.Next();
}
















/*LinkedList<int> list = new();
list.Add(1);
list.Add(2);
list.Add(3);
list.Add(4);
list.Add(5);
list.Add(6);
list.Add(7);
var iterator = list.Iterator;

while (!iterator.Complete)
{
    var n = iterator.Next();
    Console.WriteLine(n);
}
iterator.Next();

public class LinkedList<T>
{
    public Node _node { get; set; }
    
    public class Node
    {
        public Node Next { get; set; }
        public T Value { get; set; }

        public void Append(T value)
        {
            if (Next == null)
            {
                Next = new Node { Value = value };
                return;
            }
            Next.Append(value);
        }

        public T Get(int i) => i == 0 ? Value : Next.Get(--i);
    }

    public void Add(T value)
    {
        if(_node == null)
        {
            _node = new Node { Value = value };
            return;
        }
        _node.Append(value);
    }

    public T Get(int i) => _node.Get(i);

    public LinkedIterator Iterator => new LinkedIterator(_node);

    public class LinkedIterator
    {
        Node _root;
        Node _current;
        public LinkedIterator(Node root) => _root = _current = root;

        public T Next()
        {
            var Value = _current.Value;
            _current = _current.Next;
            return Value;
        }

        public bool Complete => _current == null;

        public void Reset()
        {
            _current = _root;
        }
    }
}*/