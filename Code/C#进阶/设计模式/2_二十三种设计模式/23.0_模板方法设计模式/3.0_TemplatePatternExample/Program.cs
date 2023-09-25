using _3._0_TemplatePatternExample.Templates;

namespace _3._0_TemplatePatternExample
{
    internal class Program
    {
        /*
         * 模板方法设计模式：定义一个操作中的算法骨架，而将一些步骤延迟到子类中去实现。模板方法使得 子类可以不改变一个算法的结构即可重定义该算法的某些特定步骤
         * 
         * 精华：抄试卷的方法都一样，因此直接在父类中实现防止重复，而一些特性操作，如各自的答题步骤则写成虚方法或者抽象方法，延迟到子类中去实现
         * 这里采用虚方法而不采用抽象方法分原因：虚方法可以有方法体，因此在父类中可实现，这里模仿不答题则答案为空字符串的情况
         * 
         * 意义：把不变的行为搬到父类中，去除子类中的重复代码
         * 
         * 适用场景：当不变的和可变的行为在实现时混合在一起的时候，
         *         可以用模板方法设计模式将不变的、可变的全都搬到父类中，
         *         然后由父类实现不变的，将可变的写成虚或者抽象方法，由子类延迟实现可变的
         */
        static void Main(string[] args)
        {
            Console.WriteLine("学生甲抄的试卷：");
            TestPaper studentA = new TestPaperA();
            studentA.TestQuestion1();
            studentA.TestQuestion2();
            studentA.TestQuestion3();

            Console.WriteLine("学生乙抄的试卷：");
            TestPaper studentB = new TestPaperB();
            studentB.TestQuestion1();
            studentB.TestQuestion2();
            studentB.TestQuestion3();
        }
    }
}