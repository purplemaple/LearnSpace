using _3._0_VisitorPatternExample.Actions;
using _3._0_VisitorPatternExample.Persons;

namespace _3._0_VisitorPatternExample
{
    internal class Program
    {
        /*
         * 最复杂的设计模式：访问者设计模式：表示一个作用于某对象结构中的各元素的操作。它使你可以在不改变各元素的类的前提下定义作用于这些元素的新操作
         * 
         * 意义：将处理从数据结构分离出来
         * 
         * 适用场景：有比较稳定的数据结构(如人类的性别只有两种，不会变化)，又有易于变化的算法时
         * 
         * 优点：增加新的操作很容易，因为增加新的操作就意味着增加一个新的访问者，访问者模式将有关的行为集中到一个访问者集合中
         * 
         * 缺点：使得增加新的数据结构变得困难了
         */
        static void Main(string[] args)
        {
            ObjectStructure obj = new ObjectStructure();
            obj.Attach(new Man());
            obj.Attach(new Woman());

            //成功时的反应
            Success v1 = new Success();
            obj.Display(v1);

            //失败时的反应
            Failing v2 = new Failing();
            obj.Display(v2);

            //恋爱时的反应
            Amativeness v3 = new Amativeness();
            obj.Display(v3);
        }
    }
}