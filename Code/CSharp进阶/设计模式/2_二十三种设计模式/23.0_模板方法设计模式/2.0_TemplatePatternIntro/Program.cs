using _2._0_TemplatePatternIntro.Templates;

namespace _2._0_TemplatePatternIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AbstractClass template;

            template = new ConcreteClassA();
            template.TemplateMethod();

            template = new ConcreteClassB();
            template.TemplateMethod();
        }
    }
}