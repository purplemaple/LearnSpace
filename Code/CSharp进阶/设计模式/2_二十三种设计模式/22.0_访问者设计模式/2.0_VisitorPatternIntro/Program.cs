using _2._0_VisitorPatternIntro.Elements;
using _2._0_VisitorPatternIntro.Visitors;

namespace _2._0_VisitorPatternIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ObjectStructure obj = new ObjectStructure();
            obj.Attach(new ConcreteElementA());
            obj.Attach(new ConcreteElementB());

            ConcreteVisitor1 v1 = new ConcreteVisitor1();
            ConcreteVisitor2 v2 = new ConcreteVisitor2();

            obj.Accept(v1);
            obj.Accept(v2);
        }
    }
}