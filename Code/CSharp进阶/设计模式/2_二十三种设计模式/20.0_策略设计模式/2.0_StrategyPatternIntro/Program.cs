using _2._0_StrategyPatternIntro.Strategys;

namespace _2._0_StrategyPatternIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Context context = new Context(new ConcreteStragyA());
            context.ContextInterface();

            context = new Context(new ConcreteStragyB());
            context.ContextInterface();

            context = new Context(new ConcreteStragyC());
            context.ContextInterface();
        }
    }
}